using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RulesPressurePlate : MonoBehaviour
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
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            wireManager.GenerateAll();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            wireManager.DestroyAll();
        }
    }

}
