using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

// Responsible for the audio in the game. Has default persistence through scenes, but is
// configurable to destroy itself after a certain number of transitions. This is useful for
// backgroun music.
// 
// Programmer used reference: https://www.youtube.com/watch?v=6OT43pvUyfY
public class AudioManager : MonoBehaviour
{
    [field: SerializeField] public string BackgroundMusic { get; private set; } = "";
    [field: SerializeField] public int ScenePersistence { get; private set; } = 0;
    public Sound[] Sounds;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach (Sound music in Sounds) {
            music.source = gameObject.AddComponent<AudioSource>();
            music.source.clip = music.clip;
            music.source.volume = music.vol;
            music.source.pitch = music.pitch;
            music.source.loop = music.loop;
        }
    }
    void Start()
    {
        Play(BackgroundMusic);
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
        var music = Array.Find(Sounds, sound => sound.clipName == name);
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
        var music = Array.Find(Sounds, sound => sound.clipName == name);
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
        var music = Array.Find(Sounds, sound => sound.clipName == name);
        if (music == null) {
            Debug.LogWarning("Cannot find sound " + name);
            return false;
        }
        return music.source.isPlaying;
    }
    //
    // Summary:
    //     Destroys this instance of the audio manager if it has persisted for enough scenes.
    //     This is used when background audio persistence between scenes is
    public void OnSceneTransition() {
        ScenePersistence--;
        if (ScenePersistence < 0) {
            Destroy(gameObject);
        }
    }
    //
    // List of serialised variables for a single sound to configure in scene editing.
    [System.Serializable]
    public class Sound
    {
        public string clipName;
        public AudioClip clip;
        [Range(0f,1f)] public float vol;
        [Range(0.1f, 3f)] public float pitch;
        public bool loop;
        [HideInInspector] public AudioSource source;
    }
}
