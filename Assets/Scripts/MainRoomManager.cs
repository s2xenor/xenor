using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainRoomManager : MonoBehaviour
{

    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-0.38f, -4.16f), Quaternion.identity); // Spawn master player on network
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-1.65f, -4.16f), Quaternion.identity); // Spawn player on network
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
