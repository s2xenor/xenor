using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] musics;
    private AudioSource _audioSource;
    int waitingTime = 10;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PlayMusic();
    }

    public void PlayMusic() // Play random music and wait until the end
    {
        if (_audioSource.isPlaying) return;

        int i = Random.Range(0, musics.Length);

        _audioSource.clip = musics[i];
        _audioSource.Play();
    }
}