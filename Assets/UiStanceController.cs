using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

public class UiStanceController : MonoBehaviour
{
    [Header("Child References")]
    public Image StanceImage;

    void Awake()
    {
        SubscribeToMessages();
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdateStanceMessage>(UpdateStance);
    }

    private void UpdateStance(UpdateStanceMessage msg)
    {
        StanceImage.sprite = msg.Stance.StanceSprite;
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
