using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GraphicsOptions : OptionsBase
{
    [SerializeField]
    private float brightness = 1.0f;
    public float Brightness
    {
        get { return brightness; }
        set
        {
            UpdateChangeStatus(ref brightness, value);
        }
    }

    [SerializeField]
    private Resolution currentResolution;
    public Resolution CurrentResolution
    {
        get { return currentResolution; }
        set
        {
            UpdateChangeStatus(ref currentResolution, value);

            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
        }
    }

    [SerializeField]
    private int currentQualityLevel;
    public int CurrentQualityLevel
    {
        get { return currentQualityLevel; }
        set
        {
            UpdateChangeStatus(ref currentQualityLevel, value);

            QualitySettings.SetQualityLevel(currentQualityLevel);
        }
    }

    public GraphicsOptions()
    {
    }

    public void Init()
    {
        brightness = 1.0f;

        currentResolution = new Resolution();
        currentResolution.width = Screen.width;
        currentResolution.height = Screen.height;

        currentQualityLevel = QualitySettings.GetQualityLevel();
    }

    public GraphicsOptions(GraphicsOptions go)
    {
        brightness = go.Brightness;
        currentResolution = go.CurrentResolution;
        currentQualityLevel = go.CurrentQualityLevel;
    }

    public string CurrentQualityLevelString()
    {
        return QualitySettings.names[CurrentQualityLevel];
    }
    
    public override void ApplySettings()
    {
        
        
    }
}