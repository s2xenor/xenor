using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{

    public InputField createInput;
    public InputField joinInput;

    public int sceneInt;
    public string sceneString;
    // Only one is needed

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    public override void OnJoinedRoom()
    {
        if (sceneString != null && sceneString != "")
            PhotonNetwork.LoadLevel(sceneString); // Scene to load
        else
            PhotonNetwork.LoadLevel(sceneInt); // Scene to load
    }
}
