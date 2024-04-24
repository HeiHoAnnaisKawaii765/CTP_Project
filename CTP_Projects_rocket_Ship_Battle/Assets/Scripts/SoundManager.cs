using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioSource[] bgmAudioSource;
    public List<AudioSource> sfxAudioSources;
    AudioSource currentTrack;

    [HideInInspector]
    public float bgmVolume = 0.5f;
    [HideInInspector]
    public float sfxVolume = 0.5f;
    [SerializeField]
    GameObject soundControlUI;
    private const string BGM_VOLUME_KEY = "BGM_VOLUME";
    private const string SFX_VOLUME_KEY = "SFX_VOLUME";

    private void Start()
    {
        currentTrack = bgmAudioSource[0];
        // Load the saved volume settings from PlayerPrefs, or use default values if no settings are found
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.2f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.2f);

        // Set the initial values of the sliders to the current volume levels
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        soundControlUI.SetActive(false);
        // Set the initial volume levels of the audio sources
        foreach (AudioSource bgm in bgmAudioSource)
        {
            bgm.volume = bgmVolume;
        }

        foreach (AudioSource source in sfxAudioSources)
        {
            source.volume = sfxVolume;
        }
    }
    private void Update()
    {
        foreach (AudioSource bgm in bgmAudioSource)
        {
            bgm.volume = bgmVolume;
        }

        foreach (AudioSource source in sfxAudioSources)
        {
            source.volume = sfxVolume;
        }
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        foreach (AudioSource bgm in bgmAudioSource)
        {
            bgm.volume = bgmVolume;
        }
        
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        foreach (AudioSource source in sfxAudioSources)
        {
            source.volume = sfxVolume;
        }
        sfxAudioSources[1].Play();
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
    }
    public void PlayEffectSoundButton()
    {
        sfxAudioSources[1].Play();
    }
    public void ChangeSoundTrack(int sit)
    {
        
        if(sit==0)
        {
            bgmAudioSource[0].loop = false;
            if(!bgmAudioSource[0].isPlaying)
            {
                if(!bgmAudioSource[1].isPlaying)
                {
                    bgmAudioSource[1].Play();
                    bgmAudioSource[1].loop=true;
                }
                
            }
        }
        else if (sit==1)
        {
            bgmAudioSource[1].loop = false;
            if (!bgmAudioSource[1].isPlaying)
            {
                if(!bgmAudioSource[0].isPlaying)
                {
                    bgmAudioSource[0].Play();
                    bgmAudioSource[0].loop = true;
                }
                
            }
        }
    }
    public void SoundUIControl()
    {
        if(soundControlUI.activeSelf)
        {
            soundControlUI.SetActive(false);
        }
        else
        {
            soundControlUI.SetActive(true);
        }
    }
}
