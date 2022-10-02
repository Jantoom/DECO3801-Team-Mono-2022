
/*reference: https://www.youtube.com/watch?v=6OT43pvUyfY */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Sound[] sounds;
    public static AudioManager audioInstance;


    void Awake()
    {
        if (audioInstance == null)
        {
            audioInstance = this;
            Debug.Log("null");

        } else
        {
            Destroy(gameObject);
            Debug.Log("destroy");
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound music in sounds)
        {
            music.source = gameObject.AddComponent<AudioSource>();
            music.source.clip = music.clip;
            music.source.volume = music.vol;
            music.source.pitch = music.pitch;
            music.source.loop = music.loop;
        }
    }
    void Start()
    {
        Debug.Log("aaaa");
       play("GameMusic");

    }

    public void play(string name)
    {
        Sound music = Array.Find(sounds, sound => sound.clipName == name);
        if (music == null)
        {
            Debug.LogWarning("Cannot find sound " + name);
            return;
        }
        music.source.Play();
    }
    public void stop(string name)
    {
        Sound music = Array.Find(sounds, sound => sound.clipName == name);
        if (music == null)
        {
            Debug.LogWarning("Cannot find sound " + name);
            return;
        }
        music.source.Stop();
    }

}
