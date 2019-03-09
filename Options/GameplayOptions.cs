using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class GameplayOptions : OptionsBase
{
    // DEPENDENCIES
    private CameraControl cameraControl;

    // acts as a percentage modifier
    [SerializeField]
    private float lookSensitivity;
    public float LookSensitivity
    {
        get { return lookSensitivity; }
        set
        {
            UpdateChangeStatus(ref lookSensitivity, value);
        }
    }

    [SerializeField]
    private bool invertXAxis;
    public bool InvertXAxis
    {
        get { return invertXAxis; }
        set
        {
            UpdateChangeStatus(ref invertXAxis, value);
        }
    }

    [SerializeField]
    private bool invertYAxis;
    public bool InvertYAxis
    {
        get { return invertYAxis; }
        set
        {
            UpdateChangeStatus(ref invertYAxis, value);
        }
    }

    public GameplayOptions()
    {
    }

    public void Init(CameraControl cc)
    {
        cameraControl = cc;

        if (cc != null)
        {
            lookSensitivity = cc.LookModifier;
            invertXAxis = cc.InvertX;
            invertYAxis = cc.InvertY;
        }
    }

    public GameplayOptions(GameplayOptions go)
    {
        cameraControl = go.cameraControl;
        lookSensitivity = go.lookSensitivity;
        invertXAxis = go.invertXAxis;
        invertYAxis = go.invertYAxis;
    }

    public override void ApplySettings()
    {
        // Update dependent systems with settings
        if (cameraControl != null)
        {
            cameraControl.LookModifier = lookSensitivity;
            cameraControl.InvertX = invertXAxis;
            cameraControl.InvertY = invertYAxis;
        }
    }
}