using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FetchCam : MonoBehaviour
{
    private void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>().IsMine)
            {
                gameObject.GetComponent<Canvas>().worldCamera = obj.transform.GetComponentInChildren<Camera>();
                obj.GetComponent<playerwalkOnline>().enabled = false;
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
                break;
            }
        }

        Destroy(gameObject);
    }
}
