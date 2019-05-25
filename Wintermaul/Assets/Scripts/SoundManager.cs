using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update 
    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip audioClip in clips)
        {
            audioClips.Add(audioClip.name, audioClip);
        }

        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySfx(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
    }

    private void LoadVolume()
    {
        musicSource.volume = PlayerPrefs.GetFloat("music", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }

    public void SetBackgroundMusic(string name)
    {
        musicSource.clip = audioClips[name];
        musicSource.Play();
    }
}
