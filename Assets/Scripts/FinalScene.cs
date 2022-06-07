using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class FinalScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MakeRequests());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator MakeRequests()
    {
        // GET
        var getRequest = CreateRequest("http://localhost/xenor/add.php?u1=yep&u2=yep2&score=501");
        yield return getRequest.SendWebRequest();
        //var deserializedGetData = JsonUtility.FromJson<Todo>(getRequest.downloadHandler.text);

        //// POST
        //var dataToPost = new PostData() { Hero = "John Wick", PowerLevel = 9001 };
        //var postRequest = CreateRequest("https://reqbin.com/echo/post/json", RequestType.POST, dataToPost);
        //yield return postRequest.SendWebRequest();
        //var deserializedPostData = JsonUtility.FromJson<PostResult>(postRequest.downloadHandler.text);

        //// Trigger continuation of game flow
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

    private void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }

    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }


    public class Todo
    {
        // Ensure no getters / setters
        // Typecase has to match exactly
        public int userId;
        public int id;
        public string title;
        public bool completed;
    }

    [System.Serializable]
    public class PostData
    {
        public string Hero;
        public int PowerLevel;
    }

    public class PostResult
    {
        public string success { get; set; }
    }

}