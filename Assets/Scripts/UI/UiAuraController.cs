using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

public class UiAuraController : MonoBehaviour
{
    [Header("Prefab References")]
    public UiAuraIconController AuraIconController;

    private List<AuraIcon> _icons { get; set; }

    void Awake()
    {
        _icons = new List<AuraIcon>();
        SubscribeToMessages();
    }

    public void Clear()
    {
        foreach (var icon in _icons.ToList())
        {
            _icons.Remove(icon);
            Destroy(icon.Icon.gameObject);
        }
    }

    private void OrganizeIcons()
    {
        var sortedIcons = _icons.OrderBy(i => (int) i.Parent.AuraType).ToList();
        for (var i = 0; i < sortedIcons.Count; i++)
        {
            var icon = sortedIcons[i];
            icon.Icon.transform.SetSiblingIndex(i);
        }
    }

    private void SubscribeToMessages()
    {
        gameObject.SubscribeWithFilter<AddAuraIconMessage>(AddAuraIcon, StaticFilters.CHILD);
        gameObject.SubscribeWithFilter<RemoveAuraIconMessage>(RemoveAuraIcon, StaticFilters.CHILD);
        gameObject.SubscribeWithFilter<ShowAuraCooldownMessage>(ShowAuraCooldown, StaticFilters.CHILD);
        gameObject.SubscribeWithFilter<HideAuraCooldownMessage>(HideAuraCooldown, StaticFilters.CHILD);
        gameObject.SubscribeWithFilter<ShowRadialTimerForAuraMessage>(ShowRadialTimeForAura, StaticFilters.CHILD);
        gameObject.SubscribeWithFilter<HideRaidalTimerForAuraMessage>(HideRadialTimerForAura, StaticFilters.CHILD);
    }

    private void AddAuraIcon(AddAuraIconMessage msg)
    {
        var icon = _icons.Find(i => i.Parent.Name == msg.Aura.Name);
        if (icon == null)
        {
            var controller = Instantiate(AuraIconController, transform);
            controller.Setup(msg.Aura, msg.Aura.AuraType == AuraType.Buff ? FactoryController.BUFF : FactoryController.DEBUFF);
            controller.SetStackCount(1);
            controller.SetShowStatus(false);
            _icons.Add(new AuraIcon { Parent = msg.Aura, Icon = controller, StackCount = 1});
            OrganizeIcons();
        }
        else
        {
            if (msg.Aura.MaxStack > icon.StackCount + 1)
            {
                icon.StackCount++;
                icon.Icon.SetStackCount(icon.StackCount);
            }
        }
    }

    private void RemoveAuraIcon(RemoveAuraIconMessage msg)
    {
        var icon = _icons.Find(i => i.Parent.Name == msg.Aura.Name);
        if (icon != null)
        {
            _icons.Remove(icon);
            Destroy(icon.Icon.gameObject);
            OrganizeIcons();
        }
    }

    private void ShowAuraCooldown(ShowAuraCooldownMessage msg)
    {
        if (!_icons.Exists(a => a.Parent.Name == msg.Parent.Name))
        {
            gameObject.SendMessageTo(new AddAuraIconMessage{Aura = msg.Parent }, gameObject);
        }
        var icon = _icons.Find(i => i.Parent.Name == msg.Parent.Name);
        if (msg.Time > 0)
        {
            icon?.Icon.SetCooldown(msg.Time);
        }
        else
        {
            icon?.Icon.SetShowStatus(true);
        }
    }

    private void HideAuraCooldown(HideAuraCooldownMessage msg)
    {
        var icon = _icons.Find(i => i.Parent.Name == msg.Parent.Name);
        icon?.Icon.SetShowStatus(false);
    }

    private void ShowRadialTimeForAura(ShowRadialTimerForAuraMessage msg)
    {
        if (!_icons.Exists(i => i.Parent.Name == msg.Aura.Name))
        {
            gameObject.SendMessageTo(new AddAuraIconMessage{Aura = msg.Aura}, gameObject );
        }
        var icon = _icons.Find(i => i.Parent.Name == msg.Aura.Name);
        if (msg.Time > 0)
        {
            icon?.Icon.SetRadialTimer(msg.Time, msg.FillMethod, msg.FillOrigin, msg.Start, msg.End, msg.Clockwise);
        }
        else
        {
            icon?.Icon.SetShowStatus(true);
        }
    }

    private void HideRadialTimerForAura(HideRaidalTimerForAuraMessage msg)
    {
        var icon = _icons.Find(i => i.Parent.Name == msg.Aura.Name);
        icon?.Icon.SetShowStatus(false);
    }

    void OnDestroy()
    {
        gameObject.UnsubscribeFromAllMessages();
    }
}
