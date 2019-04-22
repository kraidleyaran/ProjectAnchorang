using System.Collections.Generic;
using Assets.Scripts.System;
using Assets.Scripts.System.Input;
using MessageBusLib;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [Header("Keyboard Settings")]
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Right;

    [Header("Controller Settings")]
    [Range(0f, 1f)]
    public float LeftStickDeadZone;
    [Range(0f, 1f)]
    public float RightStickDeadZone;

    private InputState _previousState { get; set; }
    private List<GameStack<GameObject>> _registeredInputObjects { get; set; }

    void Awake()
    {
        _registeredInputObjects = new List<GameStack<GameObject>>();
        _previousState = new InputState{Buttons = new List<GameInputButton>()};
        SubscribeToMessages();
    }

    void Start()
    {
        //gameObject.SendMessage(new UpdateStickSensitivityMessage { Axis = RectTransform.Axis.Horizontal, Stick = 0, Sensitivity = LeftStickHorizontalSensitivity });
        //gameObject.SendMessage(new UpdateStickSensitivityMessage { Axis = RectTransform.Axis.Horizontal, Stick =  1, Sensitivity = RightStickHorizonalSensitivity });
        //gameObject.SendMessage(new UpdateStickSensitivityMessage { Axis = RectTransform.Axis.Vertical, Stick = 0, Sensitivity = LeftStickVeriticalSensitivity });
        //gameObject.SendMessage(new UpdateStickSensitivityMessage { Axis = RectTransform.Axis.Vertical, Stick = 1, Sensitivity =  RightStickVerticalSensitivity });
    }

    void Update()
    {
        var leftStick = new Vector2(Input.GetAxisRaw(StaticInputAxesStrings.HORIZONTAL), Input.GetAxisRaw(StaticInputAxesStrings.VERTICAL));
        var rightStick = new Vector2(Input.GetAxisRaw(StaticInputAxesStrings.RIGHT_HORIZONTAL), Input.GetAxisRaw(StaticInputAxesStrings.RIGHT_VERTICAL));

        leftStick = CheckSensitivity(leftStick, LeftStickDeadZone);
        rightStick = CheckSensitivity(rightStick, RightStickDeadZone);
        
        var currentState = new InputState
        {
            LeftStick = leftStick,
            RightStick = rightStick,
            Buttons =  new List<GameInputButton>()
        };

        var leftTrigger  = Input.GetAxis(StaticInputAxesStrings.LEFT_TRIGGER);
        if (leftTrigger > 0)
        {
            currentState.Buttons.Add(GameInputButton.LeftTrigger);
        }

        var rightTrigger = Input.GetAxis(StaticInputAxesStrings.RIGHT_TRIGGER);
        if (rightTrigger > 0)
        {
            currentState.Buttons.Add(GameInputButton.RightTrigger);
        }

        if (Input.GetButton(StaticInputAxesStrings.X))
        {
            currentState.Buttons.Add(GameInputButton.X);
        }

        if (Input.GetButton(StaticInputAxesStrings.RIGHT_BUMPER))
        {
            currentState.Buttons.Add(GameInputButton.RightBumper);
        }

        if (Input.GetButton(StaticInputAxesStrings.MENU))
        {
            currentState.Buttons.Add(GameInputButton.Menu);
        }

        if (Input.GetButton(StaticInputAxesStrings.LEFT_BUMPER))
        {
            currentState.Buttons.Add(GameInputButton.LeftBumper);
        }


        gameObject.SendMessage(new InputStateMessage{PreviousState = _previousState, CurrentState = currentState});
        //foreach (var obj in _registeredInputObjects)
        //{
        //    gameObject.SendMessageTo(inputStateMessage, obj.Item);
        //}
        _previousState = currentState;
    }

    private Vector2 CheckSensitivity(Vector2 stick, float deadZone)
    {
        if (stick.magnitude < deadZone)
        {
            stick = Vector2.zero;
        }
        return stick;

    }

    private float GetTrueSensitivity(float sensitivity)
    {
        return 1 - sensitivity;
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<SetStickSensitivityMessage>(SetStickSensitivity);
        gameObject.Subscribe<RegisterObjectForInputMessage>(RegisterObjectForInput);
        gameObject.Subscribe<UnregisterObjectForInputMessage>(UnRegisterObjectForInput);
    }

    private void SetStickSensitivity(SetStickSensitivityMessage msg)
    {
        /*
        switch (msg.Axis)
        {
            case RectTransform.Axis.Horizontal:
                if (msg.Stick > 0)
                {
                    RightStickHorizonalSensitivity = msg.Sensitivity;
                }
                else
                {
                    LeftStickHorizontalSensitivity = msg.Sensitivity;
                }
                break;
            case RectTransform.Axis.Vertical:
                if (msg.Stick > 0)
                {
                    RightStickVerticalSensitivity = msg.Sensitivity;
                }
                else
                {
                    LeftStickHorizontalSensitivity = msg.Sensitivity;
                }
                break;
        }
        gameObject.SendMessage(new UpdateStickSensitivityMessage{Axis = msg.Axis, Sensitivity = msg.Sensitivity, Stick = msg.Stick});
        */
    }

    private void RegisterObjectForInput(RegisterObjectForInputMessage msg)
    {
        var existingObject = _registeredInputObjects.Find(s => s.Item == msg.Object);
        if (existingObject != null)
        {
            existingObject.Stack++;
        }
        else
        {
            _registeredInputObjects.Add(new GameStack<GameObject>(msg.Object));
        }
    }

    private void UnRegisterObjectForInput(UnregisterObjectForInputMessage msg)
    {
        var existingObject = _registeredInputObjects.Find(s => s.Item == msg.Object);
        existingObject.Stack--;
        if (existingObject.Stack <= 0)
        {
            _registeredInputObjects.Remove(existingObject);
        }
    }
    

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
