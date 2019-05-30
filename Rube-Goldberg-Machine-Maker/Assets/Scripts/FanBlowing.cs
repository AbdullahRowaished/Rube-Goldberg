using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowing : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.SetParent(this.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            //Fan produces blowing air effect.
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 1.5f, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
        DontDestroyOnLoad(other.gameObject);
    }
}
