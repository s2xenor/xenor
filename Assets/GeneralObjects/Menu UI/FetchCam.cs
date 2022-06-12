using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;

public class FetchCam : MonoBehaviour
{
    bool del = false;
    GameObject[] monstres;

    private void Start()
    {
        monstres = GameObject.FindGameObjectsWithTag("Monster");

        foreach (var item in monstres)
        {
            item.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (del) return;

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

        foreach (var item in monstres)
        {
            item.gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }
}
