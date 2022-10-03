
/*reference: https://www.youtube.com/watch?v=6OT43pvUyfY */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using UnityEditor.SearchService;


//[System.Serializable]
public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager audioInstance;

    private UnityEngine.SceneManagement.Scene scene;



    void Awake()
    {
        Debug.Log("awake");
        scene = SceneManager.GetActiveScene();
        Debug.Log("Name: " + scene.name);

        if (audioInstance == null)
        {
            audioInstance = this;
            Debug.Log("null");

        } else
        {
            Destroy(this.gameObject);

            Debug.Log("destroy");
            return;
        }
        Debug.Log("destroylddddddddddddd");
        DontDestroyOnLoad(gameObject);
        Debug.Log("destroylooooooad");

        foreach (Sound music in sounds)
        {
            Debug.Log("music1");
            music.source = gameObject.AddComponent<AudioSource>();
            music.source.clip = music.clip;
            music.source.volume = music.vol;
            music.source.pitch = music.pitch;
            music.source.loop = music.loop;

            
        }
    }
  void Start()
    {
        Debug.Log("start");
        play("MenuMusic");

    }
   /* void Update()
    {
        Debug.Log("up");
        scene = SceneManager.GetActiveScene();
        Debug.Log("Name: " + scene.name);
        if (scene.name == "Main Menu" && !IsAudioPlaying("MenuMusic"))
        {
            play("MenuMusic");
        } else if (scene.name == "MVP" && !IsAudioPlaying("GameMusic"))
        {
            stop("MenuMusic");
            play("GameMusic");
        }
    }*/

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
            //return;
        }
        return music.source.isPlaying;

    }

    public void ChangeMusic(string music)
    {
        Debug.Log("change");
        string[] musics = music.Split(",");
            Debug.Log("1");
        Debug.Log(musics[0]);
        stop(musics[0]);

        Debug.Log("2");
        Debug.Log(musics[1]);
        play(musics[1]);

    }

    public void ChangeMusic2(string music)
    {
        Debug.Log("change2");
        string[] musics = music.Split(",");
        if (IsAudioPlaying(musics[0]))
        {
            Debug.Log("1");
            stop(musics[0]);
        }
        Debug.Log("2");
        play(musics[1]);

    }


}
