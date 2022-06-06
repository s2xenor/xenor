using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    float volume;
    float music;

    bool initVol = false;
    bool initMus = false;

    // UI Element
    public Slider volumeUI;
    public Slider musicUI;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch current settings
        volume = AudioListener.volume;

        // Apply settings to UI
        volumeUI.value = volume;


        // Fetch current settings
        music = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>().volume;

        // Apply settings to UI
        musicUI.value = music;
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
            volume = volumeUI.value;
            AudioListener.volume = volume;
        }
    }

    // Change volume
    public void Music()
    {
        if (!initMus)
        {
            initMus = true;
        }
        else
        {
            music = musicUI.value;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>().volume = music;
        }
    }
}
