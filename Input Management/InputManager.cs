using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InputState))]
public class InputManager : MonoBehaviour
{
    // Singleton
    private static InputManager inputManager;

    // Array of mappings to be interpret by the input manager
    public InputMapping[] InputMappings;
    private InputState inputState;

    // Current controller
    public ControllerType CurrentController
    {
        get
        {
            return inputState.CurrentController;
        }
    }

    private void Awake()
    {
        if (inputManager != null && inputManager != this)
        {
            Destroy(gameObject);
        }
        else if (inputManager == null)
        {
            DontDestroyOnLoad(gameObject);
            inputManager = this;
        }

        // Initialization
        inputState = GetComponent<InputState>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        // Update InputState based on current mappings
        for (int i = 0; i < InputMappings.Length; ++i)
        {
            InputMapping iMapping = InputMappings[i];
            inputState.SetButtonValue(iMapping);
        }
    }

    public static ButtonFlag GetActionFlag(GameAction actionKey)
    {
        ButtonState actionButtonState = inputManager.inputState.GetButtonState(actionKey);

        if (actionButtonState != null)
            return actionButtonState.CurrentFlag;
        else
            return ButtonFlag.UNPRESSED;
    }

    public static KeyCode GetActiveButton(GameAction actionKey)
    {
        ButtonState actionButtonState = inputManager.inputState.GetButtonState(actionKey);

        if (actionButtonState != null)
            return actionButtonState.ActiveButton;
        else
            return KeyCode.None;
    }

    // For Key is down
    public static bool GetActionValue(GameAction actionKey)
    {
        ButtonState actionButtonState = inputManager.inputState.GetButtonState(actionKey);

        if (actionButtonState != null)
            return actionButtonState.Value;
        else
            return false;
    }

    public static float GetActionNumValue(GameAction actionKey)
    {
        ButtonState actionButtonState = inputManager.inputState.GetButtonState(actionKey);

        if (actionButtonState != null)
            return actionButtonState.NumValue;
        else
            return 0.0f;
    }

    // Button down state checks
    public static bool GetActionPressed(GameAction actionKey)
    {
        ButtonState actionButtonState = inputManager.inputState.GetButtonState(actionKey);

        if (actionButtonState != null)
            return actionButtonState.CurrentFlag == ButtonFlag.PRESSED;
        else
            return false;
    }

    public static bool GetActionReleased(GameAction actionKey)
    {
        ButtonState actionButtonState = inputManager.inputState.GetButtonState(actionKey);

        if (actionButtonState != null)
            return actionButtonState.CurrentFlag == ButtonFlag.RELEASED;
        else
            return false;
    }

    public static bool GetActionHeld(GameAction actionKey)
    {
        ButtonState actionButtonState = inputManager.inputState.GetButtonState(actionKey);

        if (actionButtonState != null)
            return actionButtonState.CurrentFlag == ButtonFlag.HOLD;
        else
            return false;
    }


    // Axis
    // Joystick is hardcoded because I don't want to deal with mapping joysticks
    public static float MainHorizontal()
    {
        float r = 0.0f;

        // Add Left, Right
        ButtonState leftButton = inputManager.inputState.GetButtonState(GameAction.LEFT);
        ButtonState rightButton = inputManager.inputState.GetButtonState(GameAction.RIGHT);

        if (leftButton != null)
            r += leftButton.NumValue;

        if (rightButton != null)
            r += rightButton.NumValue;

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float MainVertical()
    {
        float r = 0.0f;

        // Add Forward, Backward
        ButtonState forwardButton = inputManager.inputState.GetButtonState(GameAction.FORWARD);
        ButtonState backwardButton = inputManager.inputState.GetButtonState(GameAction.BACKWARD);

        if (forwardButton != null)
            r += forwardButton.NumValue;

        if (backwardButton != null)
            r += backwardButton.NumValue;

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static Vector3 GetAxisInput()
    {
        return new Vector3(MainHorizontal(), 0.0f, MainVertical());
    }

    public static Vector3 GetUnitAxisInput()
    {
        return (GetAxisInput()).normalized;
    }

    public static bool GetAnyButton()
    {
        bool buttonPressed = false;

        // Iterate through all gameactions
        foreach (GameAction action in Enum.GetValues(typeof(GameAction)))
        {
            if (GetActionPressed(action))
            {
                buttonPressed = true;
                break;
            }
        }

        return buttonPressed;
    }


    // Look Axis
    public static float LookHorizontal()
    {
        float r = 0.0f;
        r += Input.GetAxis("Mouse X");
        r += Input.GetAxis("J_LookHorizontal");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static float LookVertical()
    {
        float r = 0.0f;
        r += Input.GetAxis("Mouse Y");
        r -= Input.GetAxis("J_LookVertical");

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    public static Vector3 GetLookInput()
    {
        return new Vector3(LookVertical(), LookHorizontal(), 0.0f);
    }
}
