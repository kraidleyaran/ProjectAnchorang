using System.Collections.Generic;
using Assets.Scripts.System;
using Assets.Scripts.UI;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Timed Aura", menuName = @"Anchorang/Aura/Timed")]
    public class TimedAura : Aura
    {
        [Header("Timed Aura Settings")]
        public Aura Aura;
        public float Length;
        public bool ShowUiTimer;

        private AuraController _timedAura { get; set; }
        private Sequence _timer { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _timedAura = Instantiate(FactoryController.AURA, _controller.transform.parent);
            _timedAura.Setup(Aura);
            _controller.gameObject.SendMessageTo(new AddAuraByControllerToObjectMessage{Aura = _timedAura }, _controller.transform.parent.gameObject);
            if (ShowUiTimer)
            {
                _controller.gameObject.SendMessageTo(new ShowRadialTimerForAuraMessage
                {
                    Aura = Aura,
                    Time = Length,
                    Start = 0,
                    End = 1,
                    Clockwise = true,
                    FillOrigin = FillOrigin.Top,
                    FillMethod = Image.FillMethod.Radial360
                    
                },_controller.transform.parent.gameObject);
            }
            _timer = DOTween.Sequence().AppendInterval(Length).SetEase(Ease.Linear).OnComplete(() =>
            {
                _timer = null;
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            });
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_timer != null)
            {
                _timer.Kill();
                _timer = null;
            }
            if (_timedAura)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage { Controller = _timedAura }, _controller.transform.parent.gameObject);
            }
            if (ShowUiTimer && !Aura.ShowInUi)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraIconMessage{Aura = Aura}, _controller.transform.parent.gameObject);
            }
        }
    }
}