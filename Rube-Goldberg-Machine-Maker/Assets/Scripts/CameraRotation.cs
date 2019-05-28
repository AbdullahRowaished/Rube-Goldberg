using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CameraRotation : MonoBehaviour {
    private Transform cameraRig;
    private float axis;


    private void Start()
    {
        cameraRig = GameObject.Find("VRCameraRig").transform;
    }

    private void Update()
    {
        RotateAnalog();
    }

    private void RotateAnalog()
    {
        axis = SteamVR_Actions.default_CameraRotate.GetAxis(SteamVR_Input_Sources.RightHand).x;
        if (axis > 0.4f || axis < -0.4f)
        {
            cameraRig.Rotate(Vector3.up, axis * 1.5f);
        }
    }
}
