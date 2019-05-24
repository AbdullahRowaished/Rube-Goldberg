using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowing : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            //Fan produces blowing air effect.
            Vector3 direction = transform.localRotation * transform.forward;
            Debug.Log(direction.ToString());
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Force);
        }
    }
}
