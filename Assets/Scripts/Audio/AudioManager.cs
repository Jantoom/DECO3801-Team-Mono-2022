
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
    [SerializeField]
    public Sound[] sounds;
    public static AudioManager audioInstance;

    private UnityEngine.SceneManagement.Scene scene;



    void Awake()
    {
        Debug.Log("awake");
        scene = SceneManager.GetActiveScene();
        Debug.Log("Name: " + scene.name);

       /* if (audioInstance == null)
        {
            audioInstance = this;
            Debug.Log("null");

        } else
        {
            Destroy(this.gameObject);

            Debug.Log("destroy");
            return;
        }*/
       
        //DontDestroyOnLoad(gameObject);


        foreach (Sound music in sounds)
        {
            //Debug.Log("music1");
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
        play("GameMusic");

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
        stop(musics[0]);
        play(musics[1]);

    }




}
