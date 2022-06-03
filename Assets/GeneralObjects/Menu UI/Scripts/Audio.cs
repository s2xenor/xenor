using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    float volume;
    bool initVol = false;

    // UI Element
    public Slider volumeUI;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch current video settings
        volume = AudioListener.volume;

        // Apply settings to UI
        volumeUI.value = volume;
    }

    // Change volume
    public void Volume()
    {
        if (!initVol)
        {
            initVol = true;
        }
        else
        {
            Debug.Log(volume);
            volume = volumeUI.value;
            AudioListener.volume = volume;
        }
    }
}
