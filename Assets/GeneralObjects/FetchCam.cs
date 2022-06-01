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
                break;
            }
        }
    }
}
