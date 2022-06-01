using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Video : MonoBehaviour
{
    // Every quality possible
    string[] names;

    // Has Video tab been set
    bool set = false;

    bool fullscreen;
    int quality;

    // UI elements
    public TMP_Text graphics;
    public Toggle fullscreenUI;
    public TMP_Dropdown qualityUI;

    // Start is called before the first frame update
    void Start()
    {
        names = QualitySettings.names;

        // Fetch current video settings
        fullscreen = Screen.fullScreen == true;
        quality = QualitySettings.GetQualityLevel();

        // Apply settings to menu
        fullscreenUI.isOn = fullscreen;
        qualityUI.value = quality;
    }

    // Set to fullscreen
    public void Fullscreen()
    {
        if (set)
        {
            // Toggle fullscreen
            Screen.fullScreen = !Screen.fullScreen;
            fullscreen = Screen.fullScreen == true;
        }
        else
            set = true;
    }

    public void Quality()
    {
        // Input
        string s = graphics.text;

        // Change quality
        quality = Array.IndexOf(names, s);
        QualitySettings.SetQualityLevel(quality);
    }
}
