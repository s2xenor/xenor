using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleDoorM : MonoBehaviourPunCallbacks
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            gameManager.DoorUpdate(1, false);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            gameManager.DoorUpdate(-1, false);
        }
    }
}
