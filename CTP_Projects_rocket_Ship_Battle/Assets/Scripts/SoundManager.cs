using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioSource bgmAudioSource;
    public List<AudioSource> sfxAudioSources;

    [HideInInspector]
    public float bgmVolume = 0.5f;
    [HideInInspector]
    public float sfxVolume = 0.5f;

    private const string BGM_VOLUME_KEY = "BGM_VOLUME";
    private const string SFX_VOLUME_KEY = "SFX_VOLUME";

    private void Start()
    {
        // Load the saved volume settings from PlayerPrefs, or use default values if no settings are found
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.2f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.2f);

        // Set the initial values of the sliders to the current volume levels
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        // Set the initial volume levels of the audio sources
        bgmAudioSource.volume = bgmVolume;
        foreach (AudioSource source in sfxAudioSources)
        {
            source.volume = sfxVolume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmAudioSource.volume = bgmVolume;
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
}
