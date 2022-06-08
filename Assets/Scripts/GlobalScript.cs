using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GlobalScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorUpdate(int indent, bool doubleD)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DoorUpdate(indent, doubleD);
        }
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("DoorUpdate", RpcTarget.MasterClient, indent, doubleD);
        }
    }
}
