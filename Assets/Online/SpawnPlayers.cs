using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerBoyPrefab;
    public GameObject playerGirlPrefab;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerBoyPrefab.name, new Vector2(0, 0), Quaternion.identity); // Spawn player on network
        }
        else
        {
            PhotonNetwork.Instantiate(playerGirlPrefab.name, new Vector2(0.32f, 0), Quaternion.identity); // Spawn player on network
        }
    }
}
