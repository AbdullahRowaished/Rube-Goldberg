using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public List<GameObject> menuRotatorList; //Object Menu List for each level
    public List<GameObject> starsList; //List of collectables
    public Material active, inactive; //Materials for cheating and not cheating

    private float x, y, z; //ball initial coordinates
    private Quaternion rotation; //ball initial rotation
    private int level; //currently loaded level
    private List<GameObject> collectedList; //collectables that have been collected
    private bool starsCollected; //GameLogic bool for winning


    // Use this for initialization
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y + 1;
        z = transform.position.z;
        rotation = transform.rotation;
        starsCollected = false;
        collectedList = new List<GameObject>();
        //Don't destroy on load is use for the ball and Factory to persist throughout the game.
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GameObject.Find("Factory"));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0) //if ball falls off y=0, game will be reset
        {
            ResetGame();
        }
    }

    /// <summary>
    /// Checks if ball hits ground or platform.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetGame();
        }
    }
   

    /// <summary>
    /// Collects stars and determines if player has won.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable") && !AntiCheat.cheating)
        {
            EatStar(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            if (starsCollected)
            {
                gameObject.GetComponent<Collider>().enabled = false;
                WinGame();
            }
            else
            {
                ResetGame();
            }
        }
    }
    /// <summary>
    /// Checks if player is cheating or not.
    /// </summary>
    private void CheatingIndicator()
    {
        Material mat = active;

        if (AntiCheat.cheating)
        {
            mat = inactive;
        }
        GetComponent<MeshRenderer>().material = mat;
    }
    /// <summary>
    /// Resets ball's tranform to original position and rotation.
    /// </summary>
    private void ResetBall()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.SetPositionAndRotation(new Vector3(x, y, z), rotation);
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Collider>().enabled = true;
    }
    /// <summary>
    /// Resets the game of the current level.
    /// </summary>
    private void ResetGame()
    {
        DebugManager.Info("Game Reset!");
        ResetBall();
        RemoveMachineObjects();
        ResetStars();
    }
    /// <summary>
    /// Upon resetting the game, all spawned objects will be removed.
    /// </summary>
    private void RemoveMachineObjects()
    {
        MenuRotator menuRotator = GameObject.Find("ObjectMenu").GetComponent<MenuRotator>();
        foreach (GameObject spawn in menuRotator.spawnedObjects)
        {
            DebugManager.Info(spawn.ToString());
            Destroy(spawn);
        }
        menuRotator.spawnedObjects.RemoveRange(0, menuRotator.spawnedObjects.Count);
    }
    /// <summary>
    /// Respawns collected stars.
    /// </summary>
    private void ResetStars()
    {
        if (collectedList.Count > 0)
        {
            starsList.AddRange(collectedList);
            collectedList.RemoveRange(0, collectedList.Count - 1);
            foreach (GameObject star in starsList)
            {
                star.SetActive(true);
            }
        }
    }
    /// <summary>
    /// Eats collectables to progress towards winning the game.
    /// </summary>
    /// <param name="eaten"></param>
    private void EatStar(GameObject eaten)
    {
        starsList.Remove(eaten);
        collectedList.Add(eaten);
        eaten.SetActive(false);
        if (starsList.Count == 0)
        {
            starsCollected = true;
        }
    }
    /// <summary>
    /// Wins the game.
    /// </summary>
    private void WinGame()
    {
        ResetBall();
        DebugManager.Info("Game Won!");
        if (level >= 4)
        {
            level = 0;
        }
        LoadNextScene();
    }
    /// <summary>
    /// Loads next level.
    /// </summary>
    private void LoadNextScene()
    {
        SteamVR_LoadLevel.Begin("Level" + (level + 1), true, 3);
    }
    /// <summary>
    /// Used for the delegate SceneManager.sceneLoaded. Applies everytime a new scene has loaded, including the first scene.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="lsm"></param>
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode lsm)
    {
        level++;
        collectedList = new List<GameObject>();
        starsList = new List<GameObject>();
        Destroy(GameObject.Find("ObjectMenu"));
        GameObject menuTemp = null;

        switch (scene.name)
        {
            case "Level1":
                menuTemp = Instantiate(menuRotatorList[0]);
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                break;
            case "Level2":
                menuTemp = Instantiate(menuRotatorList[1]);
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                starsList.Add(GameObject.Find("Star 3"));
                break;
            case "Level3":
                menuTemp = Instantiate(menuRotatorList[2]);
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                starsList.Add(GameObject.Find("Star 3"));
                starsList.Add(GameObject.Find("Star 4"));
                break;
            case "Level4":
                menuTemp = Instantiate(menuRotatorList[3]);
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                starsList.Add(GameObject.Find("Star 3"));
                starsList.Add(GameObject.Find("Star 4"));
                starsList.Add(GameObject.Find("Star 5"));
                break;
        }
        DebugManager.Info("Level is: " + level);
        menuTemp.name = "ObjectMenu";
        menuTemp.transform.SetParent(GameObject.Find("LeftHand").transform);
        menuTemp.transform.localPosition.Set(0, 0.2f, 0);
    }
    /// <summary>
    /// As the ball GameObject gets instantiated with the static SceneManager.sceneLoaded method call, OnLevelFinishedLoading gets called. Replacement for the deprecated OnLevelHasLoaded override.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    /// <summary>
    /// Follow-up on OnEnable.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    
}