using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapButton : MonoBehaviour
{
    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            map.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            map.SetActive(false);
        }
    }
}
