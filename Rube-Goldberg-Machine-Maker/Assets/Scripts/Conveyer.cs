using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
        speed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        //Conveyer belt activates.
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed, ForceMode.VelocityChange);
        }
    }
}
