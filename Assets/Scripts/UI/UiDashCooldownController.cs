using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

public class UiDashCooldownController : MonoBehaviour
{
    public Image DashCooldownIcon;
    private Sequence _cooldownSequence { get; set; }

    void Awake()
    {
        DashCooldownIcon.gameObject.SetActive(false);
        SubscribeToMessages();
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdateDashCooldownIconMessage>(UpdateDashCooldownIcon);
    }

    private void UpdateDashCooldownIcon(UpdateDashCooldownIconMessage msg)
    {
        if (_cooldownSequence != null && _cooldownSequence.IsActive())
        {
            _cooldownSequence.Kill();
            _cooldownSequence = null;
        }
        DashCooldownIcon.gameObject.SetActive(true);
        _cooldownSequence = DOTween.Sequence().AppendInterval(msg.Time).OnComplete(() =>
        {
            DashCooldownIcon.gameObject.SetActive(false);
        });
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}

