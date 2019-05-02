using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UiCooldownController : MonoBehaviour
{
    [Header("Internal References")]
    public Image CooldownImage;

    private Tween _cooldownTween { get; set; }
    

    public void Setup(float timer)
    {
        if (_cooldownTween != null && _cooldownTween.IsActive())
        {
            _cooldownTween.Kill();
            _cooldownTween = null;
        }
        CooldownImage.fillAmount = 1;
        _cooldownTween = CooldownImage.DOFillAmount(0, timer).SetEase(Ease.Linear).OnComplete(() =>
        {
            _cooldownTween = null;
        });
    }

    public void SetShowStatus(bool show)
    {
        if (_cooldownTween != null && _cooldownTween.IsActive())
        {
            _cooldownTween.Kill();
            _cooldownTween = null;
        }
        var fillAmount = show ? 1 : 0;
        CooldownImage.fillAmount = fillAmount;
    }
}
