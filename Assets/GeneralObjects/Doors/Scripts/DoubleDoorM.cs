using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DoubleDoorM : MonoBehaviourPunCallbacks
{
    private GameManager gameManager;
    private GameObject canvas;
    private bool txt = false;
    private int indent = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        canvas = GameObject.FindGameObjectWithTag("CanvasText");
    }



    // Update is called once per frame
    void Update()
    {
        if(txt && Input.GetKeyDown(KeyCode.E))
        {
            gameManager.DoorUpdate(1, true);
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Both player need to be on a pressure plate");
            txt = false;
            indent -= 1;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            txt = true;
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to interact with the door");
        }
        if(collision.tag == "Player" && !collision.GetComponent<PhotonView>().IsMine && PhotonNetwork.IsMasterClient)
        {
            gameManager.DoorUpdate(1, true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            gameManager.DoorUpdate(indent, true);
            txt = false;
            canvas.GetComponent<FixedTextPopUP>().SupprPressToInteractText();
        }
        if (collision.tag == "Player" && !collision.GetComponent<PhotonView>().IsMine && PhotonNetwork.IsMasterClient)
        {
            gameManager.DoorUpdate(-1, true);
        }
    }


}
