using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.up, 2);
	}
}
