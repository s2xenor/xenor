using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RulesPressurePlate : MonoBehaviourPunCallbacks
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
        if (collision.tag == "Player")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                wireManager.isOnPressureRule = true;
                if (collision.GetComponent<PhotonView>().IsMine)
                {
                    wireManager.isMasterOnRule = true;
                }
                else
                {
                    wireManager.isMasterOnRule = false;
                }
                wireManager.Changes();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                wireManager.isOnPressureRule = false;
                wireManager.Changes();
            }

        }
    }

}
