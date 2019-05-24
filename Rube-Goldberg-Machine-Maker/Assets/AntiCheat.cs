using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AntiCheat : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            //Make Ball Interactable
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            //Make Ball Not Interactable
        }
    }
}
