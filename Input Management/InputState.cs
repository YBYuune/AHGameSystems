using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ButtonFlag
{
    UNPRESSED,
    PRESSED,
    HOLD,
    RELEASED
}

public class ButtonState
{
    public InputType Type;
    public KeyCode ActiveButton;

    // pressed or not
    public bool Value;
    public float NumValue;
    public float HoldTime = 0.0f;

    // Recent values
    public ButtonFlag CurrentFlag;
    public float PrevHoldTime = 0.0f;
}

public class InputState : MonoBehaviour
{
    private Dictionary<GameAction, ButtonState> buttonStates = new Dictionary<GameAction, ButtonState>();

    private ControllerType currentController = ControllerType.KEYBOARD;
    public ControllerType CurrentController
    {
        get
        {
            return currentController;
        }
    }

    public void SetButtonValue(InputMapping iMap)
    {
        GameAction key = iMap.Action;
        InputType type = iMap.ButtonType;
        KeyCode activeButton = iMap.Key;
        ControllerType inputSource = iMap.InputSource;
        bool isToggle = iMap.IsToggle;
        bool value = iMap.Value;
        float numValue = iMap.NumValue;
        bool updateSource = iMap.WasUpdated;
        

        if (!buttonStates.ContainsKey(key))
            buttonStates.Add(key, new ButtonState());

        // Get button state from dictionary for given GameAction
        ButtonState state = buttonStates[key];

        // Check if the active source needs to be updated
        if (updateSource)
        {
            state.ActiveButton = activeButton;
            currentController = inputSource;
        }

        // If the source does not match the active source, ignore inputs
        if (state.ActiveButton != activeButton)
            return;

        // Update current press state of button
        SetActionState(ref state, ref iMap);

        // Update Values
        if (!isToggle)
        {
            // If the button is not a toggle, set value as normal
            state.Value = value;
        }
        else
        {
            // If the button is set to be a toggle, only change the value if the button was just pressed
            if (state.CurrentFlag == ButtonFlag.PRESSED)
                state.Value = !state.Value;
        }
        state.NumValue = numValue;
        state.Type = type;
    }
    

    private void SetActionState(ref ButtonState state, ref InputMapping iMap)
    {
        bool value = iMap.Value;

        // Button was on
        if (state.CurrentFlag == ButtonFlag.PRESSED || 
            state.CurrentFlag == ButtonFlag.HOLD)
        {
            // Button is being held, update HoldTime
            state.HoldTime += Time.deltaTime;

            // Update Previous Hold Time
            state.PrevHoldTime = state.HoldTime;

            if (value)
            {
                // Button is still on
                state.CurrentFlag = ButtonFlag.HOLD;
            }
            else
            {
                // Button just turned off
                state.CurrentFlag = ButtonFlag.RELEASED;

                // No longer holding down button, reset HoldTime
                state.HoldTime = 0.0f;
            }
        }

        // Button was off
        else
        {
            if (value)
            {
                // Button just turned on
                state.CurrentFlag = ButtonFlag.PRESSED;
            }
            else
            {
                // Button is still off
                state.CurrentFlag = ButtonFlag.UNPRESSED;
            }
        }
    }

    public ButtonState GetButtonState(GameAction actionKey)
    {
        if (buttonStates.ContainsKey(actionKey))
            return buttonStates[actionKey];
        else
            return null;
    }
}
