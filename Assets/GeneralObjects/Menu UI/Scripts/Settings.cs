using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Settings : MonoBehaviour
{
    // Video tab been set
    bool set = false;

    AudioSource audioData;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    // Make sound when pressed
    public void SFX()
    {
        audioData.PlayOneShot(clip);
    }

    public void Exit()
    {
        // Editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        // Build
        Application.Quit();
    }

    public void GoLobby()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GoBackToLobby();
    }

    public void Delete(GameObject obj)
    {
        Destroy(obj);
    }

    public void SendButton()
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponent<FinalScene>().ClickSend();
    }
}
