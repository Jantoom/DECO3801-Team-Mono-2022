using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

/* reference: https://www.youtube.com/watch?v=6OT43pvUyfY */
public class AudioManager : MonoBehaviour
{
    [SerializeField] public Sound[] sounds;

    void Awake()
    {
        foreach (Sound music in sounds) {
            music.source = gameObject.AddComponent<AudioSource>();
            music.source.clip = music.clip;
            music.source.volume = music.vol;
            music.source.pitch = music.pitch;
            music.source.loop = music.loop;
        }
    }
    void Start()
    {
        Play("GameMusic");
    }
    //
    // Summary:
    //     Plays the given sound in the game.
    //
    // Parameters:
    //   name:
    //     Name of the sound/music to play.
    public void Play(string name)
    {
        Sound music = Array.Find(sounds, sound => sound.clipName == name);
        if (music == null) {
            Debug.LogWarning("Cannot find sound " + name);
            return;
        }
        music.source.Play();
    }
    //
    // Summary:
    //     Stops the given sound in the game.
    //
    // Parameters:
    //   name:
    //     Name of the sound/music to stop.
    public void Stop(string name)
    {
        Sound music = Array.Find(sounds, sound => sound.clipName == name);
        if (music == null) {
            Debug.LogWarning("Cannot find sound " + name);
            return;
        }
        music.source.Stop();
    }
    //
    // Summary:
    //     Checks if a sound is currently playing in the game.
    //
    // Parameters:
    //   name:
    //     Name of the sound/music to check.
    //
    // Returns:
    //   True if the sound is playing in the game, otherwise false.
    public bool IsAudioPlaying(string name)
    {
        Sound music = Array.Find(sounds, sound => sound.clipName == name);
        if (music == null) {
            Debug.LogWarning("Cannot find sound " + name);
        }
        return music.source.isPlaying;
    }
}
