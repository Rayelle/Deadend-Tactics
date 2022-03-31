using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]
    private AudioSource[] sfxAudioSources,musicAudioSources,droneSoundAudioSources;
    [SerializeField]
    AudioClip menuMove,menuAccept,menuError;
    [SerializeField]
    float fadeStrength=0.01f,fadeInterval=0.05f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        ChangeMusicIntensity(1);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        ChangeMusicIntensity(2);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        ChangeMusicIntensity(3);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        ChangeMusicIntensity(4);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha5))
    //    {
    //        ChangeMusicIntensity(5);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        FadeInDroneSound();
    //    }
    //}
    /// <summary>
    /// Plays audioclip on audiosource without 3d-audio
    /// used for menu-soundeffects
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFXClip(AudioClip clip)
    {
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            if (!sfxAudioSources[i].isPlaying)
            {
                sfxAudioSources[i].clip = clip;
                sfxAudioSources[i].Play();
                break;
            }
        }
    }
    public void PlayMenuMoveSound()
    {
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            if (!sfxAudioSources[i].isPlaying)
            {
                sfxAudioSources[i].clip = menuMove;
                sfxAudioSources[i].Play();
                break;
            }
        }
    }
    public void PlayMenuAcceptSound()
    {
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            if (!sfxAudioSources[i].isPlaying)
            {
                sfxAudioSources[i].clip = menuAccept;
                sfxAudioSources[i].Play();
                break;
            }
        }
    }
    public void PlayMenuErrorSound()
    {
        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            if (!sfxAudioSources[i].isPlaying)
            {
                sfxAudioSources[i].clip = menuError;
                sfxAudioSources[i].Play();
                break;
            }
        }
    }
    /// <summary>
    /// fades out the volume of one music track until it reaches a volume of 0
    /// </summary>
    private IEnumerator FadeOutMusicTrack(int trackIndex)
    {
        while (musicAudioSources[trackIndex].volume > 0.01f)
        {
            musicAudioSources[trackIndex].volume -= fadeStrength;
            yield return new WaitForSeconds(fadeInterval);
        }
        musicAudioSources[trackIndex].volume = 0f;
    }  
    /// <summary>
    /// fades in the volume of one music track until it reaches a volume of maxVolume
    /// </summary>
    private IEnumerator FadeInMusicTrack(int trackIndex, float maxVolume)
    {
        if (maxVolume > 1f)
        {
            maxVolume = 1f;
        }
        while (musicAudioSources[trackIndex].volume < maxVolume-0.01f)
        {
            musicAudioSources[trackIndex].volume += fadeStrength;
            yield return new WaitForSeconds(fadeInterval);
        }
        musicAudioSources[trackIndex].volume = maxVolume;
    }
    /// <summary>
    /// change music intensity to given set up
    /// intensity must be between 1 and 5
    /// </summary>
    /// <param name="intesnsityLevel"></param>
    public void ChangeMusicIntensity(int intesnsityLevel)
    {
        FadeOutDroneSound();
        switch (intesnsityLevel)
        {
            case 1:
                StartCoroutine(FadeInMusicTrack(0, 1));
                StartCoroutine(FadeInMusicTrack(5, 1));
                StartCoroutine(FadeOutMusicTrack(1));
                StartCoroutine(FadeOutMusicTrack(2));
                StartCoroutine(FadeOutMusicTrack(3));
                StartCoroutine(FadeOutMusicTrack(4));
                break;
            case 2:
                StartCoroutine(FadeInMusicTrack(0, 1));
                StartCoroutine(FadeInMusicTrack(1, 1));
                StartCoroutine(FadeInMusicTrack(5, 1));
                StartCoroutine(FadeOutMusicTrack(2));
                StartCoroutine(FadeOutMusicTrack(3));
                StartCoroutine(FadeOutMusicTrack(4));
                break;    
            case 3:
                StartCoroutine(FadeInMusicTrack(0, 1));
                StartCoroutine(FadeInMusicTrack(1, 1));
                StartCoroutine(FadeInMusicTrack(3, 1));
                StartCoroutine(FadeInMusicTrack(5, 1));
                StartCoroutine(FadeOutMusicTrack(2));
                StartCoroutine(FadeOutMusicTrack(4));
                break;    
            case 4:
                StartCoroutine(FadeInMusicTrack(0, 1));
                StartCoroutine(FadeInMusicTrack(1, 1));
                StartCoroutine(FadeInMusicTrack(2, 1));
                StartCoroutine(FadeInMusicTrack(3, 1));
                StartCoroutine(FadeInMusicTrack(5, 1));
                StartCoroutine(FadeOutMusicTrack(4));
                break;
            case 5:
                StartCoroutine(FadeInMusicTrack(0, 1));
                StartCoroutine(FadeInMusicTrack(1, 1));
                StartCoroutine(FadeInMusicTrack(2, 1));
                StartCoroutine(FadeInMusicTrack(3, 1));
                StartCoroutine(FadeInMusicTrack(4, 1));
                StartCoroutine(FadeInMusicTrack(5, 1));
                break;
        }
    }
    /// <summary>
    /// fade out the melody so it does not interfere with the drone sound
    /// </summary>
    public void FadeOutMelody()
    {
        StartCoroutine(FadeOutMusicTrack(3));
        StartCoroutine(FadeOutMusicTrack(4));
        StartCoroutine(FadeOutMusicTrack(5));
    }
    /// <summary>
    /// fade in mysterious drone sound when for choosing upgrades
    /// </summary>
    public void FadeInDroneSound()
    {
        FadeOutMelody();
        foreach (AudioSource currentAS in droneSoundAudioSources)
        {
            StartCoroutine(FadeIn(currentAS, 1));
        }
    }
    /// <summary>
    /// fade out drone sound
    /// </summary>
    public void FadeOutDroneSound()
    {
        foreach (AudioSource currentAS in droneSoundAudioSources)
        {
            StartCoroutine(FadeOut(currentAS, 0));
        }
    }
    /// <summary>
    /// fade in a given audio source towards maxVolume
    /// </summary>
    /// <param name="toFade"></param>
    /// <param name="maxVolume"></param>
    /// <returns></returns>
    private IEnumerator FadeIn(AudioSource toFade, float maxVolume)
    {
        if (maxVolume > 1f)
        {
            maxVolume = 1f;
        }
        while (toFade.volume < maxVolume - 0.01f)
        {
            toFade.volume += fadeStrength;
            yield return new WaitForSeconds(fadeInterval);
        }
        toFade.volume = maxVolume;
    }
    /// <summary>
    /// fade out a given audio source towards minVolume
    /// </summary>
    /// <param name="toFade"></param>
    /// <param name="minVolume"></param>
    /// <returns></returns>
    private IEnumerator FadeOut(AudioSource toFade, float minVolume)
    {
        while (toFade.volume > minVolume + 0.01f)
        {
            toFade.volume -= fadeStrength;
            yield return new WaitForSeconds(fadeInterval);
        }
        toFade.volume = minVolume;
    }
}
