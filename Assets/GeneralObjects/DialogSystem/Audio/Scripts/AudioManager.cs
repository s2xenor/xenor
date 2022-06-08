using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource old_Source = null;

    public void Play(string sound)
	{
        if (old_Source != null)
        {
			old_Source.Stop();
		}
		AudioSource source = gameObject.AddComponent<AudioSource>();
		source.clip = Resources.Load<AudioClip>("Audio/" + sound);
		source.Play();
		old_Source = source;
	}
}
