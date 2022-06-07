using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SingleDoorM : MonoBehaviourPunCallbacks
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
            gameManager.DoorUpdate(1, false);
            //canvas.GetComponent<Text>().text = ("Level is not finished");
            //canvas.GetComponent<Canvas>();
            //canvas.GetComponent<Canvas>().GetComponent<Text>().text = "Level is not finished";
            //canvas.GetComponent<Canvas>().enabled = true;
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
        if (NextScene != null) gameManager.NextSceneDoor = NextScene;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            isActive = false;
            gameManager.DoorUpdate(indent, false);
            indent = 0;
            canvas.GetComponent<FixedTextPopUP>().SupprPressToInteractText();
        }
    }

}
