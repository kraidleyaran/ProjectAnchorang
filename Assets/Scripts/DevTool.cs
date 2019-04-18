using System.Collections.Generic;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevTool : MonoBehaviour
{
    [Header("Starting Player Info")]
    public List<Aura> Auras;
    public Transform StartingPosition;

    [Header("Starting Enemy Info")]
    public List<Aura> EnemyAuras;
    public Transform EnemyStartingPosition;

    [Header("Joystick Debug")]
    public bool LogJoystickInput;
    public Text LeftStickText;
    public Text RightStickText;

    void Awake()
    {
        SubscribeToMessages();
    }

    void Start()
    {
        var player = Instantiate(FactoryController.UNIT, StartingPosition.position, Quaternion.identity);
        foreach (var aura in Auras)
        {
            gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, player.gameObject);
        }

        var enemy = Instantiate(FactoryController.UNIT, EnemyStartingPosition.position, Quaternion.identity);
        foreach (var aura in EnemyAuras)
        {
            gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, enemy.gameObject );
        }
    }

    public void ResetButton()
    {
        SceneManager.LoadScene(0);
    }

    private void SubscribeToMessages()
    {
        if (LogJoystickInput)
        {
            gameObject.Subscribe<InputStateMessage>(InputState);
        }
    }

    private void InputState(InputStateMessage msg)
    {
        //LeftStickText.text = $"Left Stick {msg.CurrentState.LeftStick.x}, {msg.CurrentState.LeftStick.y}";
        //RightStickText.text = $"Right Stick {msg.CurrentState.RightStick.x}, {msg.CurrentState.RightStick.y}";
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }

}
