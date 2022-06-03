using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapButton : MonoBehaviour
{
    public GameObject map;
    bool isMapShown = false;

    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("CanvasText");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
            canvas.GetComponent<FixedTextPopUP>().PressToInteractText("Press E to see the map");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isMapShown)
                    map.SetActive(true);
                else
                    map.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            canvas.GetComponent<FixedTextPopUP>().SupprPressToInteractText();
            map.SetActive(false);
        }
    }
}
