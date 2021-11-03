using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable] public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 3f)] public float pitch = 1f;
    public bool loop;

    [HideInInspector] public AudioSource source;
}

public class FappyAudioController : MonoBehaviour
{
    public List<Sound> sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySFX(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        s?.source.Play();
    }

    public void PlayBGM(string name)
    {
        var s = sounds.Find(sound => sound.name == name);
        if (s != null && !s.source.isPlaying)
        {
            s.source.Play();
        }
    }
}
