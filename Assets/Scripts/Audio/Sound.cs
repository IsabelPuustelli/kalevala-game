using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptables/Sound")]
public class Sound : ScriptableObject
{
    public AudioType type;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;
    public bool loop;

    [HideInInspector] public AudioSource source;
}

public enum AudioType { Music, SFX }