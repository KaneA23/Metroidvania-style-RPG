using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;

	public static AudioManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;

			s.source.loop = s.loop;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		PlayAudio("SkyTheme");
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void PlayAudio(string a_name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == a_name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + a_name + " is not found!");
			return;
		}
		if (!s.source.isPlaying)
		{
			s.source.Play();
		}
	}

	public void StopAudio(string a_name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == a_name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + a_name + " is not found!");
			return;
		}
		s.source.Stop();

	}
}
