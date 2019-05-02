using System.Collections.Generic;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using Assets.Scripts.System.Input;
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
    public List<Transform> EnemyStartingPosition;
    public int EnemySpawnCount = 2;

    [Header("Joystick Debug")]
    public bool LogJoystickInput;
    public Text LeftStickText;
    public Text RightStickText;

    [Header("Color Change Debug")]
    public Color StartColor;
    public Color SwapColor;

    private Color _currentColor { get; set; }
    private UnitController _player { get; set; }

    void Awake()
    {
        SubscribeToMessages();
        _currentColor = StartColor;
    }

    void Start()
    {
        _player = Instantiate(FactoryController.UNIT, StartingPosition.position, Quaternion.identity);
        foreach (var aura in Auras)
        {
            gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _player.gameObject);
        }

        for (var i = 0; i < EnemySpawnCount; i++)
        {
            var position = Vector2.zero;
            position = i > EnemyStartingPosition.Count ? EnemyStartingPosition[EnemyStartingPosition.Count].position : EnemyStartingPosition[i].position;
            var enemy = Instantiate(FactoryController.UNIT, position, Quaternion.identity);
            foreach (var aura in EnemyAuras)
            {
                gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, enemy.gameObject);
            }
        }

    }

    public void ResetButton()
    {
        SceneManager.LoadScene(0);
    }

    public void SwapColors()
    {
        var color = SwapColor;
        if (_currentColor != StartColor)
        {
            color = StartColor;
        }
        gameObject.SendMessageTo(new ChangeSpriteColorMessage{FromColor = _currentColor, ToColor = color}, _player.gameObject);
        _currentColor = color;
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
        if (!msg.PreviousState.Buttons.Contains(GameInputButton.Menu) && msg.CurrentState.Buttons.Contains(GameInputButton.Menu))
        {
            //Debug.Break();
        }
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }

}
