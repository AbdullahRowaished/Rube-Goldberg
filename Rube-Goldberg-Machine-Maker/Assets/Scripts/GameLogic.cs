using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public List<GameObject> objectMenuList; //Object Menu List for each level
    public List<GameObject> starsList = new List<GameObject>(5); //List of collectables
    public Material active, inactive; //Materials for cheating and not cheating

    private float x, y, z; //ball initial coordinates
    private Quaternion r; //ball initial rotation
    private bool starsCollected; //GameLogic bool for winning
    private GameObject objectMenu = null;
    private Transform leftHandTransform;


    // Use this for initialization
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y + 1;
        z = transform.position.z;
        r = transform.rotation;
        starsCollected = false;

        leftHandTransform = GameObject.Find("LeftHand").transform;

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
        gameObject.transform.SetPositionAndRotation(new Vector3(x, y, z), r);
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
        MenuRotator menuRotator = objectMenu.GetComponent<MenuRotator>();
        foreach (GameObject spawn in menuRotator.spawnedObjects)
        {
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
        if (Level.value >= 4)
        {
            Level.value = 0;
        }
        LoadNextScene();
    }
    /// <summary>
    /// Loads next level.
    /// </summary>
    private void LoadNextScene()
    {
        SteamVR_LoadLevel.Begin("Level" + (Level.value + 1), false, 3);
    }
    /// <summary>
    /// Used for the delegate SceneManager.sceneLoaded. Applies everytime a new scene has loaded, including the first scene.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="lsm"></param>
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode lsm)
    {
        switch (scene.name)
        {
            case "Level1":
                Level.value = 0;
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

        if (objectMenu != null)
        {
        objectMenu.SetActive(false);
        }
        
        objectMenu = objectMenuList[Level.value];
        objectMenu.SetActive(true);
        
        Level.value++;
        //DebugManager.Info("Level to be loaded is: " + level);
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