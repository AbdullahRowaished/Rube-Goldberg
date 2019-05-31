using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static bool isDebugging = false;
    public static DebugManager debugManager;

    private void Start()
    {
        debugManager = this;
    }

    public static void Info(string message)
    {
        if (!isDebugging)
        {
            return;
        }
        else
        {
            Debug.Log(message);
        }
    }
}
