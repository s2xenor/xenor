using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    float volume;

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
        volume = GameObject.Find("Master Volume").GetComponent<Slider>().value;
        AudioListener.volume = volume;
    }
}
