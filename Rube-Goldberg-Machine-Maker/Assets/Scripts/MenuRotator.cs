using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MenuRotator : MonoBehaviour {
    public List<GameObject> objectList;
    public List<GameObject> objectPrefabList;
    public List<GameObject> spawnedObjects;

    private int currentObject, timeSpawned;
    private float axis;
    private bool swiping;
    private int counter = 0;

    // Use this for initialization
    void Start () {
        currentObject = 0;

        foreach (Transform child in transform)
        {
            objectList.Add(child.gameObject);
        }

        swiping = false;

        spawnedObjects = new List<GameObject>();
	}

    // Update is called once per frame
    void Update()
    {
        Swipe();
        Spawn();
    }

    private void MenuLeft()
    {
        objectList[currentObject].SetActive(false);

        if (currentObject <= 0)
        {
            currentObject = objectList.Count - 1;
        } else
        {
            currentObject--;
        }

        objectList[currentObject].SetActive(true);
    }

    private void MenuRight()
    {
        objectList[currentObject].SetActive(false);

        if (currentObject >= objectList.Count - 1)
        {
            currentObject = 0;
        }
        else
        {
            currentObject++;
        }

        objectList[currentObject].SetActive(true);
    }

    private void Swipe()
    {
        axis = SteamVR_Actions.default_MenuSwipe.GetAxis(SteamVR_Input_Sources.LeftHand).x;

        if (!swiping)
        {
            if (axis > 0.5f)
            {
                MenuLeft();
                swiping = true;
            }
            else if (axis < -0.5f)
            {
                MenuRight();
                swiping = true;
            }
            else
            {
                swiping = false;
            }
        }
        else
        {
            swiping = axis > 0.5 || axis < -0.5;
        }
    }

    private void Spawn()
    {
        if (SteamVR_Actions.default_SpawnObject.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            int maxSpawn = 0, totalSpawn = 0;

            if (objectList[currentObject].name.Equals("Plank") || objectList[currentObject].name.Equals("Metal Plank"))
            {
                maxSpawn = Level.value + 2;
            } else if (objectList[currentObject].name.Equals("Cardboard") || objectList[currentObject].name.Equals("Cabinet"))
            {
                maxSpawn = Level.value + 1;
            } else if (objectList[currentObject].name.Equals("Robotic Arm"))
            {
                maxSpawn = Level.value;
            } else if (objectList[currentObject].name.Equals("Trampoline"))
            {
                maxSpawn = Level.value - 1;
            } else if (objectList[currentObject].name.Equals("Fan"))
            {
                maxSpawn = Level.value - 2;
            } else if (objectList[currentObject].name.Equals("Conveyer"))
            {
                maxSpawn = Level.value - 3;
            }

            DebugManager.Info("maxSpawn of " + objectList[currentObject].name + ": " + maxSpawn);

            foreach (GameObject item in spawnedObjects)
            {
                if (item.name.Contains(objectList[currentObject].name))
                {
                    totalSpawn++;
                }
            }

            DebugManager.Info("totalSpawn of " + objectList[currentObject].name + ": " + totalSpawn);

            if (totalSpawn >= maxSpawn)
            {
                return;
            }

            foreach (GameObject item in objectPrefabList)
            {
                if (item.name.Equals(objectList[currentObject].name))
                {
                    spawnedObjects.Add(Instantiate(item, transform.position, transform.rotation));
                }
            }
        }
    }
}
