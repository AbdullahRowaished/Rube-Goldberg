using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public List<GameObject> objectMenuPrefabs; //Object Menu List for each level
    public List<GameObject> starsList = new List<GameObject>(); //List of collectables
    public Material active, inactive; //Materials for cheating and not cheating

    private float x, y, z; //ball initial coordinates
    private Quaternion rotation; //ball initial rotation
    private int level; //currently loaded level
    private bool starsCollected; //GameLogic bool for winning
    private List<GameObject> objectMenuList;
    private GameObject objectMenu = null;


    // Use this for initialization
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y + 1;
        z = transform.position.z;
        rotation = transform.rotation;
        starsCollected = false;
        objectMenuList = new List<GameObject>(4);
        for (int i = 0; i < objectMenuPrefabs.Count; i++)
        {
            objectMenuList[i] = Instantiate(objectMenuPrefabs[i]);
        }

        //Don't destroy on load is use for the ball and Factory to persist throughout the game.
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GameObject.Find("Factory"));
        DontDestroyOnLoad(GameObject.Find("Ground"));
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
        RemoveObjects();
        ResetStars();
    }
    /// <summary>
    /// Upon resetting the game, all spawned objects will be removed.
    /// </summary>
    private void RemoveObjects()
    {
        //DebugManager.Info("ObjectMenu: " + GameObject.Find("ObjectMenu").name);
        //DebugManager.Info("MenuRotator: " + GameObject.Find("ObjectMenu").GetComponent<MenuRotator>().name);
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
        foreach (GameObject star in starsList)
        {
            star.SetActive(true);
        }
    }
    /// <summary>
    /// Eats collectables to progress towards winning the game.
    /// </summary>
    /// <param name="eaten"></param>
    private void EatStar(GameObject eaten)
    {
        eaten.SetActive(false);
        starsCollected = true;
        foreach (GameObject star in starsList)
        {
            
            if (star.activeSelf)
            {
                starsCollected = false;
                break;
            }
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

        if (objectMenu != null)
        {
            objectMenu.SetActive(false);
        }

        objectMenu = objectMenuList[level];
        Transform parent = GameObject.Find("LeftHand").transform;
        objectMenu.transform.SetParent(transform);
        objectMenu.transform.SetPositionAndRotation(transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
        objectMenu.SetActive(true);

        switch (scene.name)
        {
            case "Level1":
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                break;
            case "Level2":
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                starsList.Add(GameObject.Find("Star 3"));
                break;
            case "Level3":
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                starsList.Add(GameObject.Find("Star 3"));
                starsList.Add(GameObject.Find("Star 4"));
                break;
            case "Level4":
                starsList.Add(GameObject.Find("Star 1"));
                starsList.Add(GameObject.Find("Star 2"));
                starsList.Add(GameObject.Find("Star 3"));
                starsList.Add(GameObject.Find("Star 4"));
                starsList.Add(GameObject.Find("Star 5"));
                break;
        }
        DebugManager.Info("Level is: " + level);
        

        level++;
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