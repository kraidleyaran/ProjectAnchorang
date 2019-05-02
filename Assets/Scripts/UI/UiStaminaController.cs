using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

public class UiStaminaController : MonoBehaviour
{
    private const string STAMINA_LABEL = "Stamina";

    [Header("Child References")]
    public Image BarBacking;
    public Image BarAmount;


    void Awake()
    {
        SubscribeToMessages();
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdatePlayerStaminaMessage>(UpdatePlayerStamina);
    }

    private void UpdatePlayerStamina(UpdatePlayerStaminaMessage msg)
    {
        var percent = msg.CurrentStamina / msg.MaximumStamina;
        BarAmount.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * BarBacking.rectTransform.rect.width);
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
