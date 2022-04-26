using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
<<<<<<< HEAD

=======
>>>>>>> 756454f287492ea84c6c430f3a4988132070c5c4

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-1,-1), Quaternion.identity); // Spawn player on network
    }
}
