using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameAction
{
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT,
    JUMP,
    THROW,
    AIM,
    INV_LEFT,
    INV_RIGHT,
    INTERACT,
    SUPPORT,
    SNEAK,
    PAUSE,
    ACCEPT,
    CANCEL,
    CANCEL_SUPPORT
}

public enum InputType
{
    BUTTON,
    AXIS
}

public enum ControllerType
{
    KEYBOARD,
    GAMEPAD
}

public enum Condition
{
    GREATER_THAN,
    LESS_THAN
}

[System.Serializable]
public class InputMapping
{
    // Name that will appear in the array view of the editor viewport
    public string Name;

    // Each input will be associated with an action that will control stuff
    public GameAction Action;

    // Only certain variables will be used depending on the input type
    public InputType ButtonType;

    // Enum for determining if the UI should show Gamepad/Keyboard inputs
    public ControllerType InputSource;

    // Toggle-able inputs will only change on key press
    public bool IsToggle;

    // <Button> KeyCode referring to the button that will activate the
    //     action
    public KeyCode Key;

    // <Button> Value to return upon button press
    public float KeyDownValue;

    // <Axis> If the input type is an axis, the name will refer to the axis
    //     to query Unity's input manager
    public string AxisName;

    // <Axis> Condition which states whether the associated action should be
    //     considered enabled
    public Condition AxisCondition;

    // <Axis> Action will be considered true if the condition is true in relation to the
    //     DeadZone value
    public float DeadZone;

    // this will help us know if either keyboard or gamepad updated the input
    private bool wasUpdated;
    public bool WasUpdated
    {
        get
        {
            bool updated = wasUpdated;

            // clear if true
            if (wasUpdated)
                wasUpdated = false;

            return updated;
        }
    }

    private bool value;
    public bool Value
    {
        get
        {
            bool newValue = false;

            // Get Input from Unity's Input Manager
            // If the input type is of an axis, we compare it against its deadzone value
            switch (ButtonType)
            {
                case InputType.BUTTON:
                    newValue = Input.GetKey(Key);
                    break;
                case InputType.AXIS:
                    newValue = GetAxisNumValue() != 0.0f;
                    break;
            }

            // if value is changing, the gameaction was updated
            if (value != newValue)
                wasUpdated = true;

            value = newValue;
            return value;
        }
    }

    public float NumValue
    {
        get
        {
            switch(ButtonType)
            {
                case InputType.BUTTON:
                    if (Value)
                    {
                        return KeyDownValue * 1.0f;
                    }
                    break;
                case InputType.AXIS:
                    return GetAxisNumValue();
            }

            return 0.0f;
        }
    }

    // Helper function to get the value from the actual axis
    private float GetAxisNumValue()
    {
        // Sanity Check
        if (ButtonType != InputType.AXIS)
            return 0.0f;

        float val = Input.GetAxis(AxisName);

        // Only return axis value if its condition is true
        switch (AxisCondition)
        {
            case Condition.GREATER_THAN:
                if (val > DeadZone)
                    return val;
                break;
            case Condition.LESS_THAN:
                if (val < DeadZone)
                    return val;
                break;
        }

        return 0.0f;
    }
}
