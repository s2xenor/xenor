using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public TMP_Text graphics;

    // Every quality possible
    string[] names;

    // Video tab been set
    bool set = false;

    AudioSource audioData;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        names = QualitySettings.names;
        audioData = GetComponent<AudioSource>();
    }

    // Set to fullscreen
    public void Fullscreen()
    {
        if (set)
        {
            // Toggle fullscreen
            Screen.fullScreen = !Screen.fullScreen;
        }
        else
            set = true;
    }

    public void Quality()
    {
        // Input
        string s = graphics.text;

        // Change quality
        QualitySettings.SetQualityLevel(Array.IndexOf(names, s));
    }

    // Make sound when pressed
    public void SFX()
    {
        Debug.Log("ok");
        audioData.PlayOneShot(clip);
    }
}
