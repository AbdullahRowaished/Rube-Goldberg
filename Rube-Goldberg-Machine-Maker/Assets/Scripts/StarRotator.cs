using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotator : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.up, 2);
	}
}
