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
            mat = active;
        }
        else
        {
            mat = inactive;
        }

        if (appliedMAT.name.Equals(mat.name))
        {
            return;
        }
        else
        {
            ball.GetComponent<MeshRenderer>().material = mat;
        }
    }

}
