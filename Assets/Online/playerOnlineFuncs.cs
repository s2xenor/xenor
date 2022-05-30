using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerOnlineFuncs : MonoBehaviour
{
    // Set child camera as camera for canvas
    public void MazeCanvas()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().worldCamera = transform.GetChild(0).gameObject.GetComponent<Camera>();
    }
}
