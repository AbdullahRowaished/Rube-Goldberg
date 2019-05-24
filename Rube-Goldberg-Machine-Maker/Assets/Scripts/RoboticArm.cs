using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm : MonoBehaviour {
    private bool rotating, rotated;
    private int rotations;
    

	// Use this for initialization
	void Start () {
        rotating = false;
        rotated = false;
        rotations = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && (!rotating || !rotated))
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.SetParent(gameObject.transform);
            rotating = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (rotating && rotations < 180)
        {
            gameObject.transform.Rotate(gameObject.transform.up, 1);
            rotations++;
        } else
        {
            rotating = false;
            rotated = true;
            gameObject.transform.DetachChildren();
            if (other.gameObject.CompareTag("Ball"))
            {
                other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
