using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class OptionsContainer
{
    public bool OptionsHaveChanged
    {
        get
        {
            // If any of the options have changed, then we need to update the current options
            if (CurrentGraphicsOptions.OptionsHaveChanged ||
                CurrentAudioOptions.OptionsHaveChanged || 
                CurrentGameplayOptions.OptionsHaveChanged)
            {
                return true;
            }

            return false;
        }
    }

    // DEPENDENCIES
    [SerializeField]
    private CameraControl cameraControl;

    [SerializeField]
    private AudioMixer masterMixer;


    // Reference to new settings object. If we apply the new settings, we absorb their fields
    public OptionsContainer NewOptions;

    public GraphicsOptions CurrentGraphicsOptions = new GraphicsOptions();
    public AudioOptions CurrentAudioOptions = new AudioOptions();
    public GameplayOptions CurrentGameplayOptions = new GameplayOptions();

    public OptionsContainer()
    {
    }

    public void Init()
    {
        if (cameraControl == null)
        {
            // Try to find camera control
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            if (camera != null)
            {
                CameraControl cc = camera.GetComponent<CameraControl>();

                if (cc != null)
                {
                    cameraControl = cc;
                }
            }
        }

        NewOptions = null;

        CurrentGraphicsOptions.Init();
        CurrentAudioOptions.Init(masterMixer);
        CurrentGameplayOptions.Init(cameraControl);
    }

    public OptionsContainer(OptionsContainer oc)
    {
        oc.NewOptions = this;

        CurrentGraphicsOptions = new GraphicsOptions(oc.CurrentGraphicsOptions);
        CurrentAudioOptions = new AudioOptions(oc.CurrentAudioOptions);
        CurrentGameplayOptions = new GameplayOptions(oc.CurrentGameplayOptions);
    }

    public void ApplyChanges()
    {
        // If this is the OptionsContainer, then we have nothing to apply
        if (NewOptions == null)
            return;

        // if no options have been changed, there's nothing to apply
        if (!NewOptions.OptionsHaveChanged)
            return;


        // Update Graphics options to new settings if they've changed
        if (NewOptions.CurrentGraphicsOptions.OptionsHaveChanged)
        {
            // Old Graphics options are now the new ones
            CurrentGraphicsOptions = NewOptions.CurrentGraphicsOptions;

            // Change Unity settings
            CurrentGraphicsOptions.ApplySettings();
        }

        // Update Audio options to new settings if they've changed
        if (NewOptions.CurrentAudioOptions.OptionsHaveChanged)
        {
            CurrentAudioOptions = NewOptions.CurrentAudioOptions;

            CurrentAudioOptions.ApplySettings();
        }

        // Update Gameplay options to new settings if they've changed
        if (NewOptions.CurrentGameplayOptions.OptionsHaveChanged)
        {
            CurrentGameplayOptions = NewOptions.CurrentGameplayOptions;

            CurrentGameplayOptions.ApplySettings();
        }
    }
}
