using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

public class UiHealthController : MonoBehaviour
{
    [Header("Child References")]
    public Image BarBacking;
    public Image BarAmount;

    void Awake()
    {
        SubscribeToMessages();
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdatePlayerHealthMessage>(UpdatePlayerHealth);
    }

    private void UpdatePlayerHealth(UpdatePlayerHealthMessage msg)
    {
        var percent = (float) msg.CurrentHealth / msg.MaximumHealth;
        BarAmount.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * BarBacking.rectTransform.rect.width);
        //AmountText.text = $@"{msg.CurrentHealth}/{msg.MaximumHealth}";
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
