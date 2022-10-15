
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;



//[System.Serializable]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public Sound[] sounds;



    void Awake()
    {

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
        play("GameMusic1");

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

    public bool IsAudioPlaying(string name)
    {

        Sound music = Array.Find(sounds, sound => sound.clipName == name);
        if (music == null)
        {
            Debug.LogWarning("Cannot find sound " + name);
        }
        return music.source.isPlaying;

    }



}
