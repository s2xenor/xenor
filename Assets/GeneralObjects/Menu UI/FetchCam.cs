using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;

public class FetchCam : MonoBehaviour
{
    Light2D light; // Global Light
    float intentsity = 1; // Original intensity of the light
    bool del = false;

    void Update()
    {
        if (del) return;

        Debug.Log(GameObject.FindGameObjectsWithTag("GlobalLight").Length);
        light = GameObject.FindGameObjectsWithTag("GlobalLight")[0].GetComponent<Light2D>();

        if (light.intensity != 1)
            intentsity = light.intensity;
        light.intensity = 1;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>().IsMine)
            {
                gameObject.GetComponent<Canvas>().worldCamera = obj.transform.GetComponentInChildren<Camera>(); // Assign player camera to canvas
                obj.GetComponent<playerwalkOnline>().enabled = false; // Disable moving
                obj.GetComponentInChildren<Light2D>().enabled = false; // Disable lights
                break;
            }
        }
    }

    public void Del()
    {
        del = true;

        Debug.Log("delete loading");
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>().IsMine)
            {
                obj.GetComponent<playerwalkOnline>().enabled = true;
                obj.GetComponentInChildren<Light2D>().enabled = true;
                break;
            }
        }

        if (light)
        {
            light.intensity = intentsity;
        }

        Destroy(gameObject);
    }
}
