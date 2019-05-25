using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    private bool playerInside, ballInside;

    private void Update()
    {
        AntiCheat.cheating = !playerInside || !ballInside;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        } else if (other.CompareTag("Throwable"))
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
        else if (other.CompareTag("Throwable"))
        {
            ballInside = false;
        }
    }
}
