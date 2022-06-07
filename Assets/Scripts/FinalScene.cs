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

public class FinalScene : MonoBehaviourPunCallbacks
{

    public GameObject finalScreen;
    GameManager gameManager;
    private bool canLoad = false;
    private string user1;
    private string user2;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.TimestampEnd = (int)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;

    }

    // Update is called once per frame
    void Update()
    {
        if (canLoad && Input.GetKeyDown(KeyCode.Escape))
        {
            Instantiate(finalScreen, Vector2.zero, Quaternion.identity);
        }
    }

    

    //set username of player
    [PunRPC]
    public void SetUsername(string username, bool master = true)
    {
        if (master) user1 = username;
        else user2 = username;

        if (user1 != null && user2 != null)
        {
            StartCoroutine(MakeRequests());
        }
    }

    private IEnumerator MakeRequests()
    {
        string url = $"http://xenor.usiobe.com/xenor/add.php?u1={user1}&u2={user2}&score={CalculateScore().ToString()}";
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
            SetUsername(GameObject.FindGameObjectWithTag("Username").GetComponent<InputField>().text, true);
        }
        else
        {
            PhotonView photonview = GetComponent<PhotonView>();
            photonview.RPC("SetUsername", RpcTarget.MasterClient, GameObject.FindGameObjectWithTag("Username").GetComponent<InputField>().text, false);
        }

    }

    public void SetScore(int score, bool local = false)
    {
        if(local) this.score = score;
        else
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SetScore", RpcTarget.All, score, true);
        }

    }

}