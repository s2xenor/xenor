using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnObj : MonoBehaviour
{
    public GameObject[] obj;
    public int nb;
    public bool random;
    public bool spawnAtStart = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Update()
    {
        if (spawnAtStart)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (random && Random.Range(0, 2) == 0) return;

            for (int i = 0; i < nb; i++)
            {
                PhotonNetwork.Instantiate(obj[Random.Range(0, obj.Length)].name, transform.position, Quaternion.identity);
            }
            spawnAtStart = false;
        }
    }
}
