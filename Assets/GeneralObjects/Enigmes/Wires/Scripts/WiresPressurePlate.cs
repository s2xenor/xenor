using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WiresPressurePlate : MonoBehaviour
{
    public WiresManager wireManager;
    bool show = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (show && wireManager.isOn)
        {
            show = false;
            foreach (GameObject plug in wireManager.plugsL)
            {
                plug.SetActive(true);
            }
            foreach (GameObject plug in wireManager.plugsN)
            {
                plug.SetActive(true);
            }
            foreach (GameObject wire in wireManager.wires)
            {
                wire.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            show = true;
        }
    }

}
