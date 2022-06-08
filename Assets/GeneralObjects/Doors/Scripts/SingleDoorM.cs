using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SingleDoorM : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject canvas;

    public string NextScene;

    private bool isActive = false;
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
        if (isActive && Input.GetKeyDown(KeyCode.E))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                gameManager.DoorUpdate(1, false);
                if (NextScene != null) gameManager.NextSceneDoor = NextScene;
            }
            else GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalScript>().DoorUpdate(1, false);

            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Level is not finished");
            indent -= 1;
            isActive = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            isActive = true;
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to interact with the door");
        }
        if(collision.tag == "Player") //setup nextscene door for client for him and overwrite it if is one other door in update
        {
            if (NextScene != null) gameManager.NextSceneDoor = NextScene;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            isActive = false;
            if (PhotonNetwork.IsMasterClient) gameManager.DoorUpdate(indent, false);
            else GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalScript>().DoorUpdate(indent, false);
            
            indent = 0;
            canvas.GetComponent<FixedTextPopUP>().SupprPressToInteractText();
        }
    }

}
