using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoubleDoorM : MonoBehaviourPunCallbacks
{
    private GameManager gameManager;
    public GameObject canvas;
    private bool txt = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Instantiate(canvas, new Vector2(0, 0), Quaternion.identity);
    }



    // Update is called once per frame
    void Update()
    {
        if(txt && Input.GetKeyDown(KeyCode.E))
        {
            DoorUpdate(1);
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Both player need to be on a pressure plate");
            txt = false;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            txt = true;
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to interact with the door");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DoorUpdate(-1);
            txt = false;
            canvas.GetComponent<FixedTextPopUP>().SupprPressToInteractText();
        }
    }

    [PunRPC]
    public void DoorUpdate(int increment)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameManager.DoorUpdate(increment, true);
        }
        else
        {
            PhotonView p = this.GetComponent<PhotonView>();
            p.RPC("DoorUpdate", RpcTarget.MasterClient, increment);

        }
    }

}
