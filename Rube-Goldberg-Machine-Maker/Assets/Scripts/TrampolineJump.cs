using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineJump : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //Trampoline produces jumping effect.
            collision.gameObject.GetComponent<Rigidbody>().AddForce(0, 1, 0, ForceMode.Impulse);
        }
    }
}
