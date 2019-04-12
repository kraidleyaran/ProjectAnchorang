using System.Collections.Generic;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Timed Aura", menuName = @"Anchorang/Aura/Timed")]
    public class TimedAura : Aura
    {
        [Header("Timed Aura Settings")]
        public Aura Aura;
        public float Length;

        private AuraController _timedAura { get; set; }
        private Sequence _timer { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _timedAura = Instantiate(FactoryController.AURA, _controller.transform.parent);
            _timedAura.Setup(Aura);
            _controller.gameObject.SendMessageTo(new AddAuraByControllerToObjectMessage{Aura = _timedAura }, _controller.transform.parent.gameObject);
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
        }
    }
}