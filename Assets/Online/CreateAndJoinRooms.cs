using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomString;

    public int sceneInt;
    public string sceneString;
    // Only one is needed

    public void CreateRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.CreateRoom(roomString.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRoom(roomString.text);

    }

    public override void OnJoinedRoom()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager.continuePrevGame)
        {
            PhotonNetwork.LoadLevel("MainRoom"); // load main room
        }
        else //load default scene usually cells
        {
            if (sceneString != null && sceneString != "")
                PhotonNetwork.LoadLevel(sceneString); // Scene to load
            else
                PhotonNetwork.LoadLevel(sceneInt); // Scene to load
        }
    }
}
