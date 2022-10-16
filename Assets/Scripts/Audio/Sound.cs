
/* reference: https://www.youtube.com/watch?v=6OT43pvUyfY */

using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string clipName;
    public AudioClip clip;
    

    [Range(0f,1f)]
    public float vol;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;    



}
