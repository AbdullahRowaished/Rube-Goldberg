using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowing : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            //Fan produces blowing air effect.
            Vector3 direction = transform.localRotation * transform.forward;
            DebugManager.Info(direction.ToString());
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Force);
        }
    }
}
