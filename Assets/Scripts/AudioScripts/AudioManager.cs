using System;
using UnityEngine;

/// <summary>
/// Controls what sound is played when.
/// Created by: Kane Adams
/// </summary>
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

    /// <summary>
    /// Looks for a sound to play and begins playing it if not already
    /// </summary>
    /// <param name="a_name">New sound to be played</param>
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

    /// <summary>
    /// Looks for a sound being played and stops it
    /// </summary>
    /// <param name="a_name">Sound to stop playing</param>
    public void StopAudio(string a_name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == a_name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + a_name + " is not found!");
            return;
        }
        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }
}
