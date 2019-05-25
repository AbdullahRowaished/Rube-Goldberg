using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    private bool playerInside, ballInside;
    public Material active, inactive;
    private Material mat, appliedMAT;
    private GameObject ball;

    private void Start()
    {
        ball = GameObject.Find("Ball");
        appliedMAT = ball.GetComponent<MeshRenderer>().material;
        mat = null;
    }

    private void Update()
    {
        AntiCheatCheck();
        DebugManager.Info(playerInside + " " + ballInside);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        } else if (other.CompareTag("Ball"))
        {
            ballInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
        else if (other.CompareTag("Ball"))
        {
            ballInside = false;
        }
    }

    private void AntiCheatCheck ()
    {
        AntiCheat.cheating = !playerInside && !ballInside;

        if (AntiCheat.cheating)
        {
            DebugManager.Info("Cheating: " + AntiCheat.cheating);
            mat = active;
            DebugManager.Info("Active: " + mat.name);
        }
        else
        {
            mat = inactive;
            DebugManager.Info("Inactive: " + mat.name);
        }

        if (appliedMAT.name.Equals(mat.name))
        {
            DebugManager.Info("No Change in Material");
            return;
        }
        else
        {
            DebugManager.Info("Material Changed");
            ball.GetComponent<MeshRenderer>().material = mat;
        }
    }

}
