using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        speed = 0.05f;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Conveyer belt activates.
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddRelativeTorque(transform.forward * speed, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.transform.SetParent(null);
            DontDestroyOnLoad(other.gameObject);
        }
    }
}
