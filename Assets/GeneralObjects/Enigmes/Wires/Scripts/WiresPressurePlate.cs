using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WiresPressurePlate : MonoBehaviour
{
    public WiresManager wireManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if walk on pressure plate and has wire show them
        if (collision.tag == "Player")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                wireManager.isOnPressureWire= true;
                if (collision.GetComponent<PhotonView>().IsMine)
                {
                    wireManager.isMasterOnWire = true;
                }
                else
                {
                    wireManager.isMasterOnWire = false;
                }
                wireManager.Changes();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if walk on pressure plate and has wire show them
        if (collision.tag == "Player")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                wireManager.isOnPressureWire = false;
                wireManager.Changes();
            }
        }
    }

}
