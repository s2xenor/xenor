using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HitboxPlayer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("Reduce2", RpcTarget.All, 1);
        }
    }
}
