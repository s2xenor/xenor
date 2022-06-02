using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Awake()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(0, 0), Quaternion.identity); // Spawn player on network
    }
}
