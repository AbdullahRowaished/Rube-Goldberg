using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFixer : MonoBehaviour {
    public GameObject dest, inv, anticheat;

    // Update is called once per frame
    void Update () {
        if (dest.activeSelf || inv.activeSelf)
        {
            anticheat.SetActive(false);
        } else if (!anticheat.activeSelf)
        {
            anticheat.SetActive(true);
        }
	}
}
