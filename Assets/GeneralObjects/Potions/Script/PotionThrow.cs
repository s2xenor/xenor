using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class PotionThrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && !collision.isTrigger)
        {

            Destroy(gameObject, .05f);
        }
        else if (collision.tag == "Player" && !collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            Destroy(gameObject, .05f);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 5);//destroy the object on the map 

        playerwalkOnline walk = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            if (go.GetComponent<PhotonView>().IsMine)
                walk = go.GetComponent<playerwalkOnline>();

        Vector2 mouvement = new Vector2(Input.GetAxis(walk.horizon), Input.GetAxis(walk.verti));//create a mouvement for the potion
        mouvement.Normalize();
        this.GetComponent<Rigidbody2D>().velocity = mouvement * 3;//throw the potion
    }
}
