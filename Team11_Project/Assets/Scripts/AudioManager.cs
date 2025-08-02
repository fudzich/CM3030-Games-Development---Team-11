using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioType
    {
        Levelup,
        Melee_Attack,
        Ranged_Attack,
        Walk,
        Player_Damage,
        Enemy_Death,
        Menu_Buttons,
        Win,
        Invisibility,
        Skill,
        Background_Music,
        Background_Sounds
    }
 
    [System.Serializable]
    public class Audio
    {
        public AudioType Type;
        public AudioClip Clip;
 
        [Range(0f, 1f)]
        public float Volume = 1f;
 
        [HideInInspector]
        public AudioSource Source;
    }
 
    public static AudioManager Instance;
 

    public Audio[] AllAudios;
 
    private Dictionary<AudioType, Audio> _audioDictionary = new Dictionary<AudioType, Audio>();
    private AudioSource _musicSource;
 
    private void Awake()
    {
        //Assign singleton
        Instance = this;
 
        //Set up audios
        foreach(var s in AllAudios)
        {
            _audioDictionary[s.Type] = s;
        }
    }
 
 
 
    //method to play an audio
    public void Play(AudioType type)
    {

        if (!_audioDictionary.TryGetValue(type, out Audio s))
        {
            Debug.LogWarning($"Audio type {type} not found!");
            return;
        }
 
        //Creates a new audio object
        var audioObj = new GameObject($"Audio_{type}");
        var audioSrc = audioObj.AddComponent<AudioSource>();
 
        //Assigns your audio properties
        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume;
 
        //Play the audio
        audioSrc.Play();
 
        //Destroy the object
        Destroy(audioObj, s.Clip.length);
    }
 
    //method to change music tracks
    public void PlayMusic(AudioType type)
    {
        if (!_audioDictionary.TryGetValue(type, out Audio track))
        {
            Debug.LogWarning($"Music track {type} not found!");
            return;
        }
 
        //if (_musicSource == null)
        //{
            var container = new GameObject("AudioTrackObj");
            _musicSource = container.AddComponent<AudioSource>();
            _musicSource.loop = true;
        //}
 
        _musicSource.clip = track.Clip;
        _musicSource.volume = track.Volume;
        _musicSource.Play();
    }
}
