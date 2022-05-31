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
                wireManager.SetOnPressureRules(true);
                wireManager.GenerateAll(true);
            }
            else
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SetOnPressureRules", RpcTarget.MasterClient, true);
                photonView.RPC("GenerateAll", RpcTarget.MasterClient, false);

            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                wireManager.SetOnPressureRules(false);
            }
            else
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("SetOnPressureRules", RpcTarget.MasterClient, false);

            }
            //wireManager.DestroyAll();
        }
    }

}
