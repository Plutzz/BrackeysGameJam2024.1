using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonPersistent<AudioManager>
{
    [Header("Sounds")]
    public SoundAudioClip[] soundAudioClipsArray;
    public AudioMixerGroup sfxMixerGroup;

    [SerializeField]
    private Queue<GameObject> soundAudioClipsQueue;
    private GameObject musicGameObject;

    [SerializeField]
    private int maxAudioSources = 10;

    [Header("Music")]
    public SongAudioClip[] musicAudioClipsArray;
    public AudioMixerGroup musicMixerGroup;
    


    public enum Sounds
    {
        DuckWalk,
        DoorGet,
        Door_Borg,
        Door_Gorg,
        Lever,
        Box,
        Jump,
        Stalactite,

    }

    public enum Songs
    {
        MainMenu,
        HaroldsPerplection,
        SeaOfCircuts
    }

    private void Start()
    {
        PlaySong(Songs.MainMenu);
    }

    /// <summary>
    /// Use this for sounds that may be repeated very quickly Ex: a bunch of towers shooting
    /// </summary>
    /// <param name="_sound"></param>
    public void PlaySound(Sounds _sound)
    {
        if (soundAudioClipsQueue == null)
        {
            soundAudioClipsQueue = new Queue<GameObject>(maxAudioSources);
        }

        Debug.Log("Number of Audio Sources: " + soundAudioClipsQueue.Count);

        GameObject soundGameObject;
        AudioSource audioSource;
        //Create Audio Source Game Object
        if (soundAudioClipsQueue.Count < maxAudioSources)
        {
            soundGameObject = new GameObject("Sound");
            soundAudioClipsQueue.Enqueue(soundGameObject);
            audioSource = soundGameObject.AddComponent<AudioSource>();
            
        }
        else
        {
            soundGameObject = soundAudioClipsQueue.Dequeue();
            soundAudioClipsQueue.Enqueue(soundGameObject);
            audioSource = soundGameObject.GetComponent<AudioSource>();
            audioSource.Stop();
        }
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.clip = GetAudioClip(_sound).audioClip;
        audioSource.volume = GetAudioClip(_sound).volume;
        audioSource.PlayOneShot(audioSource.clip);
    }

    /// <summary>
    /// Use this for sound effects where you want to hear the entire sound effect ex: Lightning
    /// WARNING: using this with sound effects that are repeated often may result in broken audio
    /// </summary>
    /// <param name="_sound"></param>

    public void PlayEntireSound(Sounds _sound)
    {
        //Create Audio Source Game Object
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(_sound).audioClip;
        audioSource.volume = GetAudioClip(_sound).volume;
        audioSource.priority = 50;
        audioSource.PlayOneShot(audioSource.clip);
        Destroy(soundGameObject, audioSource.clip.length);
    }

    /// <summary>
    /// Changes the currently playing song
    /// </summary>
    /// <param name="_sound"></param>
    /// <returns></returns>
    public void PlaySong(Songs _song)
    {
        if (musicGameObject == null)
        {
            musicGameObject = new GameObject("Music");
            musicGameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(musicGameObject);
        }
        AudioSource audioSource = musicGameObject.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = musicMixerGroup;

        audioSource.Stop();
        audioSource.clip = GetAudioClip(_song).audioClip;
        audioSource.volume = GetAudioClip(_song).volume;
        audioSource.priority = 10;
        audioSource.loop = true;
        audioSource.Play();
    }

    private SoundAudioClip GetAudioClip(Sounds _sound)
    {
        foreach (SoundAudioClip soundAudioClip in soundAudioClipsArray)
        {
            if (soundAudioClip.sound == _sound)
            {
                return soundAudioClip;
            }
        }

        Debug.LogError("Sound " + soundAudioClipsArray + "not found!");
        return null;
    }

    private SongAudioClip GetAudioClip(Songs _song)
    {
        foreach (SongAudioClip songAudioClip in musicAudioClipsArray)
        {
            if (songAudioClip.song == _song)
            {
                return songAudioClip;
            }
        }

        Debug.LogError("Song " + musicAudioClipsArray + "not found!");
        return null;
    }

    [Serializable]
    public class SoundAudioClip
    {
        public Sounds sound;
        public AudioClip audioClip;

        [SerializeField, Range(0f, 1f)]
        public float volume = .5f;
    }

    [Serializable]
    public class SongAudioClip
    {
        public Songs song;
        public AudioClip audioClip;

        [SerializeField, Range(0f, 1f)]
        public float volume = .5f;
    }


}