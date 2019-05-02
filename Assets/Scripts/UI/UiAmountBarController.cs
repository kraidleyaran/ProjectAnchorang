using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UiAmountBarController : MonoBehaviour
    {
        [Header("Child References")]
        public Image BackingImage;
        public Image FillImage;
        public Image SecondaryFill;
        public float SecondaryBarLength = 157.7f;
        //public Image SecondaryStartBar;
        public Text AmountText;

        void Awake()
        {

        }

        public void SetFillPerecent(float percent)
        {
            var setPercent = percent;
            if (setPercent > 1)
            {
                setPercent = 1;
            }
            
            FillImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, setPercent * BackingImage.rectTransform.rect.width);
        }

        public void SetSecondaryFill(float percent)
        {
            var setPercent = percent;
            if (setPercent > 1)
            {
                setPercent = 1;
            }
            SecondaryFill.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, setPercent * SecondaryBarLength);
        }

        public void SetAmount(float current, int maximum, bool showDecimal = false)
        {
            AmountText.text = showDecimal ? $"{current:0.#} / {maximum}" : $"{(int) current} / {maximum}";
        }

        public void SetFillBarColor(Color color)
        {
            if (FillImage.color != color)
            {
                FillImage.color = color;
            }
        }
    }
}