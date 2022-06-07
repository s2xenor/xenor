using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleDoorM : MonoBehaviourPunCallbacks
{
    private GameManager gameManager;
    public GameObject canvas;

    public string NextScene;

    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Instantiate(canvas, new Vector2(0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.E))
        {
            DoorUpdate(1);
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Level is not finished");
            isActive = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            isActive = true;
            if (NextScene != null) gameManager.NextSceneDoor = NextScene;
            DoorUpdate(1);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            isActive = false;
            DoorUpdate(-1);
            canvas.GetComponent<FixedTextPopUP>().SupprPressToInteractText();
        }
    }

    [PunRPC]
    public void DoorUpdate(int increment)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameManager.DoorUpdate(increment, false);
        }
        else
        {
            PhotonView p = this.GetComponent<PhotonView>();
            p.RPC("DoorUpdate", RpcTarget.MasterClient, increment);

        }
    }
}
