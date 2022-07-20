using System;
using UnityEngine;
using UnityEngine.Audio;

// Static class for managing audio
// Usage: AudioManager.Play("name");

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer mixer;
    public Sound[] sounds;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        LoadSounds();
    }

    // Loads all sounds from the sounds array
    void LoadSounds()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixer.FindMatchingGroups(s.type.ToString())[0];
        }
    }
    // Plays a sound by name
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s)
            s.source.Play();
        else
            Debug.LogWarning("Sound: " + name + " not found!");
    }

    // Stops a sound by name
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s)
            s.source.Pause();
        else
            Debug.LogWarning("Sound: " + name + " not found!");
    }

    // Stops a sound by name
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s)
            s.source.Stop();
        else
            Debug.LogWarning("Sound: " + name + " not found!");
    }

}