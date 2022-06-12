using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class PotionThrow : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;

    public int owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && !collision.isTrigger)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1);
            source.clip = clip;
            source.Play();
        }
        else if (collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1);
            source.clip = clip;
            source.Play();
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

        if (mouvement == Vector2.zero)
            mouvement = new Vector2(1, 0);

        mouvement.Normalize();
        this.GetComponent<Rigidbody2D>().velocity = mouvement * 3;//throw the potion
    }

    [PunRPC]
    public void SetOwner(int id)
    {
        owner = id;
    }
}
