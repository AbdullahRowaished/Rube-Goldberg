using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineJump : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //Trampoline produces jumping effect.
            collision.gameObject.GetComponent<Rigidbody>().AddForce(0, 1, 0, ForceMode.Impulse);
        }
    }
}
