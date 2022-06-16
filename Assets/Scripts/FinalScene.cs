using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviourPunCallbacks
{

    public GameObject finalScreen;
    GameManager gameManager;
    private bool canLoad = false;
    private string userPlayer1;
    private string userPlayer2;
    private int score = 5;

    public GameObject playerBoyPrefab;
    public GameObject playerGirlPrefab;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.TimestampEnd = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        gameManager.CalculateScore();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerBoyPrefab.name, new Vector2(0.93f, -0.29f), Quaternion.identity); // Spawn master player on network
        }
        else
        {
            PhotonNetwork.Instantiate(playerGirlPrefab.name, new Vector2(-0.49f, -0.456f), Quaternion.identity); // Spawn player on network
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canLoad && Input.GetKeyDown(KeyCode.Escape))
        {
            Instantiate(finalScreen, Vector2.zero, Quaternion.identity);
            Invoke("SetFct", 0.5f);
            canLoad = false;
        }
    }

    private void SetFct()
    {
        GameObject.FindGameObjectWithTag("Box").GetComponentInChildren<TMP_Text>().SetText($"F�licitations vous avez fini Altervita !\nUn classement des scores est disponbile sur le site du repo du projet (https://github.com/s2xenor/xenor)\nVotre score est : {score}\nPour le sauvergarder et l'envoyer dans le tableau des scores, veuillez entrer un username :");
    }



    //set username of player
    [PunRPC]
    public void SetUsername(string username, bool master = true)
    {
        if (master) userPlayer1 = username;
        else userPlayer2 = username;

        if (userPlayer1 != null && userPlayer2 != null)
        {
            if (score < 0) score = 0;
            StartCoroutine(MakeRequests());
            Invoke("Finished2", 0.5f);
        }
    }

    public void Finished2()
    {
        Finished();
    }

    [PunRPC]
    public void Finished(bool local = false)
    {
        if (local)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("AlteraVitaMenu");
        }
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Finished", RpcTarget.All, true);
        }
    }

    private IEnumerator MakeRequests()
    {
        string url = "http://xenor.usiobe.com/add.php?u1="+ userPlayer1 + "&u2="+ userPlayer2+ "&score="+score;
        var getRequest = CreateRequest(url);
        yield return getRequest.SendWebRequest();
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }


    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }

    //set username after button send his click
    public void ClickSend()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetUsername(GameObject.FindGameObjectWithTag("Username").transform.GetChild(2).GetComponent<Text>().text, true);
        }
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SetUsername", RpcTarget.MasterClient, GameObject.FindGameObjectWithTag("Username").transform.GetChild(2).GetComponent<Text>().text, false);
        }

    }

    [PunRPC]
    public void SetScore(int score, bool local = false)
    {
        if (local)
        {
            this.score = score;
            canLoad = true;
        }
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SetScore", RpcTarget.All, score, true);
        }

    }

}