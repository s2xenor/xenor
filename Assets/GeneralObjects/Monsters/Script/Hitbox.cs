using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.GetChild(0).GetComponent<PhotonView>().RPC("Reduce2", RpcTarget.All, 1);//reduce player life
        }
    }
}
