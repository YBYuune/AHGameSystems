using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



[System.Serializable]
public class AudioOptions : OptionsBase
{
    // DEPENDENCIES
    private AudioMixer masterMixer;

    [SerializeField]
    private float masterVolume;
    public float MasterVolume
    {
        get { return masterVolume; }
        set
        {
            UpdateChangeStatus(ref masterVolume, value);

            masterMixer.SetFloat("MasterVolume", masterVolume);
        }
    }

    [SerializeField]
    private float bgmVolume;
    public float BGMVolume
    {
        get { return bgmVolume; }
        set
        {
            UpdateChangeStatus(ref bgmVolume, value);

            masterMixer.SetFloat("BGMVolume", bgmVolume);
        }
    }

    [SerializeField]
    private float sfxVolume;
    public float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            UpdateChangeStatus(ref sfxVolume, value);

            masterMixer.SetFloat("SFXVolume", sfxVolume);
            masterMixer.SetFloat("GameSFXVolume", sfxVolume);
        }
    }

    public AudioOptions()
    {
    }

    public void Init(AudioMixer master)
    {
        masterMixer = master;

        masterMixer.GetFloat("MasterVolume", out masterVolume);
        masterMixer.GetFloat("BGMVolume", out bgmVolume);
        masterMixer.GetFloat("SFXVolume", out sfxVolume);
    }


    public AudioOptions(AudioOptions ao)
    {
        masterMixer = ao.masterMixer;

        masterVolume = ao.masterVolume;
        bgmVolume = ao.bgmVolume;
        sfxVolume = ao.sfxVolume;
    }

    public override void ApplySettings()
    {
        masterMixer.SetFloat("MasterVolume", masterVolume);
        masterMixer.SetFloat("BGMVolume", bgmVolume);
        masterMixer.SetFloat("SFXVolume", sfxVolume);
        masterMixer.SetFloat("GameSFXVolume", sfxVolume);
    }
}