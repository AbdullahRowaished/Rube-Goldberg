using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotation : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        Rotate();
	}

    private void Rotate()
    {
        gameObject.transform.Rotate(0,0,20);
    }

    
}
