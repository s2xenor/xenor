using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;

public class FetchCam : MonoBehaviour
{
    Light2D light; // Global Light
    float intentsity; // Original intensity of the light

    private void LateStart()
    {
        light = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
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
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>().IsMine)
            {
                obj.GetComponent<playerwalkOnline>().enabled = true;
                obj.GetComponentInChildren<Light2D>().enabled = true;
                break;
            }
        }

        light.intensity = intentsity;

        Destroy(gameObject);
    }
}
