using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnObjects : MonoBehaviour
{
    // All objects must be in Resources folder
    public List<GameObject> OBJECTS;

    private void Start()
    {
        foreach (GameObject obj in OBJECTS)
            PhotonNetwork.Instantiate(obj.name, new Vector2(-1, -1), Quaternion.identity); // Spawn object on network
    }
}
