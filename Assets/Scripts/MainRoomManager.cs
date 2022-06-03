using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainRoomManager : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefab;
    public GameObject DoorsOpenCrate;
    public GameObject DoorsOpenPipe;
    public GameObject DoorsOpenLabyInvisible;
    public GameObject DoorsOpenArrows;
    public GameObject DoorsOpenWires;
    public GameObject DoorsOpenDonjon;

    public GameObject Doors1;
    public GameObject Doors2;
    public GameObject Doors3;
    public GameObject Doors4;
    public GameObject Doors5;
    public GameObject Doors6;


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

    [PunRPC]
    public void SetDoors(bool Crate, bool Pipe, bool LabyInvisible, bool Arrows, bool Wires, bool Donjon, bool global)
    {
        if (global)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SetDoors", RpcTarget.All, Crate, Pipe, LabyInvisible, Arrows, Wires, Donjon, false);
        }
        else
        {
            int count = 0;
            if (Crate) DoorsOpenCrate.SetActive(true); count++;
            if (Pipe) DoorsOpenCrate.SetActive(true); count++;
            if (LabyInvisible) DoorsOpenCrate.SetActive(true); count++;
            if (Arrows) DoorsOpenCrate.SetActive(true); count++;
            if (Wires) DoorsOpenCrate.SetActive(true); count++;
            if (Donjon) DoorsOpenCrate.SetActive(true); count++;

            if (count == 1) Doors1.SetActive(true);
            else if (count == 2) Doors2.SetActive(true);
            else if (count == 3) Doors3.SetActive(true);
            else if (count == 4) Doors4.SetActive(true);
            else if (count == 5) Doors5.SetActive(true);
            else if (count == 6) Doors6.SetActive(true);


        }

    }
}
