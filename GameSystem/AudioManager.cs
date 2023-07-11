using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in Sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.pitch = s.pitch;
            s.audioSource.volume= s.volume;
        }
    }


	public void Play(string name)
	{
        Sound s = Array.Find(Sounds, (sound) => sound.name == name);
        s.audioSource.Play();
	}

	public void PlayOneShot(string name)
	{
		Sound s = Array.Find(Sounds, (sound) => sound.name == name);
        s.audioSource.PlayOneShot(s.audioSource.clip);
	}

    public void Stop(string name)
    {
		Sound s = Array.Find(Sounds, (sound) => sound.name == name);
        s.audioSource.Stop();
	}

    public void CheckPlay(string name)
    {
		Sound s = Array.Find(Sounds, (sound) => sound.name == name);
        if(!s.audioSource.isPlaying)
        {
            s.audioSource.Play();
        }
	}
}
