using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapButton : MonoBehaviour
{
    public GameObject map;

    bool isMapShown = false;

    Collider2D playerTrigger = null; // My player when on trigger
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("CanvasText");
    }

    private void Update()
    {
        if (playerTrigger != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!isMapShown)
                map.SetActive(true);
            else
                map.SetActive(false);

            isMapShown = !isMapShown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to see the map");
            playerTrigger = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            canvas.GetComponent<FixedTextPopUP>().SupprPressToInteractText();
            map.SetActive(false);
            playerTrigger = null;
            isMapShown = false;
        }
    }
}
