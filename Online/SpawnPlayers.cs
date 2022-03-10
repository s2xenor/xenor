using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject maze;

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-1,-1), Quaternion.identity); // Spawn player on network
    }
}
