using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Auras;
using Assets.Scripts.System;
using Assets.Scripts.UI;
using MessageBusLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiAuraIconController : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [Header("Child References")]
    public Image Background;
    public Image Icon;
    public Text StackCount;
    public UiRaidalTimerController RadialTimerController;

    public Aura Parent { get; private set; }

    public void Setup(Aura parent, Color background)
    {
        Parent = parent;
        Icon.sprite = Parent.Sprite;
        Background.color = background;
    }

    public void SetCooldown(float time)
    {
        RadialTimerController.Setup(time, Image.FillMethod.Radial360, FillOrigin.Top, 1, 0);
    }

    public void SetRadialTimer(float time, Image.FillMethod fillMethod, FillOrigin fillOrigin, float start, float end, bool clockwise = false)
    {
        RadialTimerController.Setup(time, fillMethod, fillOrigin, start,end, clockwise);
    }

    public void SetShowStatus(bool shown)
    {
        RadialTimerController.SetShownStatus(shown);
    }

    public void SetStackCount(int count)
    {
        StackCount.text = count > 1 ? $"{count}" : string.Empty;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.SendMessage(new ShowAuraDescriptionMessage{Aura = Parent});
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SendMessage(new ClearAuraDescriptionMessage());
    }
}
