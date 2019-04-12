using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebugController : MonoBehaviour
{
    [Header("Child References")]
    public InputField PlayerAcceleration;
    public InputField PlayerMaxSpeed;

    void Awake()
    {
        SubscribeToMessages();
        PlayerAcceleration.onValidateInput += ValidateText;
        PlayerAcceleration.onValidateInput += ValidateText;
    }

    public void UpdatePlayerAcceleration()
    {
        gameObject.SendMessage(new SetPlayerAccelerationMessage{Acceleration = float.Parse(PlayerAcceleration.text)});
    }

    public void UpdatePlayerMaxSpeed()
    {
        gameObject.SendMessage(new SetPlayerMaxSpeedMessage{MaxSpeed = float.Parse(PlayerMaxSpeed.text)});
    }

    private char ValidateText(string text, int charIndex, char addedChar)
    {
        var actualText = text.Insert(charIndex, $"{addedChar.ToString()}");
        if (float.TryParse(actualText, out var number) && number >= 0f)
        {
            return addedChar;
        }
        return '\0';
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdatePlayerInfoMessage>(UpdatePlayerInfo);
    }

    private void UpdatePlayerInfo(UpdatePlayerInfoMessage msg)
    {
        if (float.TryParse(PlayerAcceleration.text, out var acceleration))
        {
            if (acceleration != msg.Acceleration)
            {
                PlayerAcceleration.text = $"{acceleration}";
            }
        }
        else
        {
            PlayerAcceleration.text = $"{msg.Acceleration}";
        }
        if (float.TryParse(PlayerMaxSpeed.text, out var maxSpeed))
        {
            if (maxSpeed != msg.MaxSpeed)
            {
                PlayerMaxSpeed.text = $"{maxSpeed}";
            }
        }
        else
        {
            PlayerMaxSpeed.text = $"{msg.MaxSpeed}";
        }
    }



    
}
