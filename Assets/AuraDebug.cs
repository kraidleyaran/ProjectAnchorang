using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

public class AuraDebug : MonoBehaviour
{
    [Header("Child References")]
    public Text NameText;
    public Text DescriptionText;

    private Aura _aura { get; set; }

    void Awake()
    {
        SubscribeToMessages();
    }

    void Update()
    {
        if (_aura)
        {
            DescriptionText.text = _aura.AuraDescription;
        }
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<ShowAuraDescriptionMessage>(ShowAuraDescription);
        gameObject.Subscribe<ClearAuraDescriptionMessage>(ClearAuraDescription);
    }

    private void ShowAuraDescription(ShowAuraDescriptionMessage msg)
    {
        _aura = msg.Aura;
        NameText.text = msg.Aura.Name;
    }

    private void ClearAuraDescription(ClearAuraDescriptionMessage msg)
    {
        _aura = null;
        NameText.text = string.Empty;
        DescriptionText.text = string.Empty;
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
