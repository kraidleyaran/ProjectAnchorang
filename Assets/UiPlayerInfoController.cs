using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.System;
using Assets.Scripts.UI;
using MessageBusLib;
using UnityEngine;

public class UiPlayerInfoController : MonoBehaviour
{
    [Header("Player Info Settings")]
    public Color HasEnoughStaminaColor;
    public Color NotEnoughStaminaColor;

    [Header("Child References")]
    public UiAmountBarController PlayerHealth;
    public UiAmountBarController PlayerStamina;
    public UiAuraController AuraController;

    void Awake()
    {
        SubscribeToMessages();
    }

    private void SubscribeToMessages()
    {
        gameObject.Subscribe<UpdatePlayerHealthMessage>(UpdatePlayerHealth);
        gameObject.Subscribe<UpdatePlayerStaminaMessage>(UpdatePlayerStamina);
        gameObject.Subscribe<UpdateRequiredStaminamStatusMessage>(UpdateRequireStaminaStatus);
        gameObject.Subscribe<AddPlayerAuraIconMessage>(AddPlayerAuraIcon);
        gameObject.Subscribe<RemovePlayerAuraIconMessage>(RemovePlayerAuraIcon);
        gameObject.Subscribe<ShowPlayerAuraCooldownMessage>(ShowPlayerAuraCooldown);
        gameObject.Subscribe<HidePlayerAuraCooldownMessage>(HidePlayerAuraCooldown);
        gameObject.Subscribe<ShowRadialTimerForPlayerAuraMessage>(ShowRadialTimerForPlayerAura);
        gameObject.Subscribe<HideRaidlTimerForPlayerMessage>(HideRadialTimerForPlayerAura);
    }

    private void UpdatePlayerHealth(UpdatePlayerHealthMessage msg)
    {
        PlayerHealth.SetFillPerecent((float)msg.CurrentHealth / msg.MaximumHealth);
        //PlayerHealth.SetAmount(msg.CurrentHealth, msg.MaximumHealth);
    }


    private void UpdatePlayerStamina(UpdatePlayerStaminaMessage msg)
    {
        var baseStamina = msg.CurrentStamina;
        var extraStamina = 0f;
        if (baseStamina > 100)
        {
            extraStamina = baseStamina - 100;
            baseStamina -= extraStamina;
        }

        PlayerStamina.SetFillPerecent(baseStamina / msg.MaximumStamina);
        PlayerStamina.SetSecondaryFill(extraStamina / 50);
        //PlayerStamina.SetAmount(msg.CurrentStamina, msg.MaximumStamina, true);
    }

    private void UpdateRequireStaminaStatus(UpdateRequiredStaminamStatusMessage msg)
    {
        var color = msg.HasEnough ? HasEnoughStaminaColor : NotEnoughStaminaColor;
        PlayerStamina.SetFillBarColor(color);
    }

    private void AddPlayerAuraIcon(AddPlayerAuraIconMessage msg)
    {
        gameObject.SendMessageTo(new AddAuraIconMessage{Aura = msg.Aura}, AuraController.gameObject);
    }

    private void RemovePlayerAuraIcon(RemovePlayerAuraIconMessage msg)
    {
        gameObject.SendMessageTo(new RemoveAuraIconMessage{Aura = msg.Aura}, AuraController.gameObject);
    }

    private void ShowPlayerAuraCooldown(ShowPlayerAuraCooldownMessage msg)
    {
        gameObject.SendMessageTo(new ShowAuraCooldownMessage{Parent = msg.Aura, Time = msg.Time}, AuraController.gameObject);
    }

    private void HidePlayerAuraCooldown(HidePlayerAuraCooldownMessage msg)
    {
        gameObject.SendMessageTo(new HideAuraCooldownMessage{Parent = msg.Aura}, AuraController.gameObject);
    }

    private void ShowRadialTimerForPlayerAura(ShowRadialTimerForPlayerAuraMessage msg)
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

    private void HideRadialTimerForPlayerAura(HideRaidlTimerForPlayerMessage msg)
    {
        gameObject.SendMessageTo(new HideRaidalTimerForAuraMessage(), AuraController.gameObject);
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
