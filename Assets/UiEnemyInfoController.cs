using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.System;
using Assets.Scripts.UI;
using MessageBusLib;
using UnityEngine;

public class UiEnemyInfoController : MonoBehaviour
{
    [Header("Child References")]
    public UiAmountBarController EnemyHealth;
    public UiAuraController AuraController;

    void Awake()
    {
        SubscribeToMessages();
    }

    public void Clear()
    {
        AuraController.Clear();
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdateEnemyHealthMessage>(UpdateEnemyHealth);
        gameObject.Subscribe<AddEnemyAuraIconMessage>(AddEnemyAuraIcon);
        gameObject.Subscribe<RemoveEnemyAuraIconMessage>(RemoveEnemyAuraIcon);
        gameObject.Subscribe<ShowEnemyCooldownAuraMessage>(ShowEnemyCooldownAura);
        gameObject.Subscribe<HideEnemyCooldownAuraMessage>(HideEnemyCooldownAura);
        gameObject.Subscribe<ShowRaidalTimerForEnemyAuraMessage>(ShowRadialTimerForEnemyAura);
        gameObject.Subscribe<HideRadialTimerForEnemyAuraMessage>(HideRaidalTimerForEnemyAura);
    }

    private void UpdateEnemyHealth(UpdateEnemyHealthMessage msg)
    {
        EnemyHealth.SetFillPerecent((float)msg.CurrentHealth / msg.MaxHealth);
    }

    private void AddEnemyAuraIcon(AddEnemyAuraIconMessage msg)
    {
        gameObject.SendMessageTo(new AddAuraIconMessage{Aura = msg.Aura}, AuraController.gameObject);
    }

    private void RemoveEnemyAuraIcon(RemoveEnemyAuraIconMessage msg)
    {
        gameObject.SendMessageTo(new RemoveAuraIconMessage{Aura = msg.Aura}, AuraController.gameObject);
    }

    private void ShowEnemyCooldownAura(ShowEnemyCooldownAuraMessage msg)
    {
        gameObject.SendMessageTo(new ShowAuraCooldownMessage{Parent = msg.Aura, Time = msg.Time}, AuraController.gameObject);
    }

    private void HideEnemyCooldownAura(HideEnemyCooldownAuraMessage msg)
    {
        gameObject.SendMessageTo(new HideEnemyCooldownAuraMessage { Aura = msg.Aura }, AuraController.gameObject);
    }

    private void ShowRadialTimerForEnemyAura(ShowRaidalTimerForEnemyAuraMessage msg)
    {
        gameObject.SendMessageTo(new ShowRadialTimerForAuraMessage
        {
            Aura = msg.Aura,
            Time = msg.Time,
            Start = msg.Start,
            End = msg.End,
            Clockwise = msg.Clockwise,
            FillOrigin = msg.FillOrigin,
            FillMethod = msg.FillMethod
        }, AuraController.gameObject);
    }

    private void HideRaidalTimerForEnemyAura(HideRadialTimerForEnemyAuraMessage msg)
    {
        gameObject.SendMessageTo(new HideRaidalTimerForAuraMessage{Aura = msg.Aura}, AuraController.gameObject);
    }
    

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
