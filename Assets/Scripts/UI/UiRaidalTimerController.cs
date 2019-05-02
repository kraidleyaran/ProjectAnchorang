using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UiRaidalTimerController : MonoBehaviour
    {
        [Header("Chlid References")]
        public Image RadialImage;

        private Tween _fillTween { get; set; }

        public void Setup(float timer, Image.FillMethod fillmethod, FillOrigin fillOrigin, float start, float end, bool clockwise = false)
        {
            RadialImage.fillAmount = start;
            RadialImage.fillMethod = fillmethod;
            RadialImage.fillOrigin = (int) fillOrigin;
            RadialImage.fillClockwise = clockwise;
            _fillTween = RadialImage.DOFillAmount(end, timer).SetEase(Ease.Linear).OnComplete(() =>
            {
                _fillTween = null;
            });
        }

        public void SetShownStatus(bool shown)
        {
            RadialImage.fillAmount = shown ? 1 : 0;
        }
    }
}