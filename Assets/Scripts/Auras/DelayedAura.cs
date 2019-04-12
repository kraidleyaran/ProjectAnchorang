using System.Collections.Generic;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Delayed Aura", menuName = @"Anchorang/Aura/Delayed")]
    public class DelayedAura : Aura
    {
        public List<Aura> Auras;
        public float WaitTime;

        private Sequence _timer { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _timer = DOTween.Sequence().AppendInterval(WaitTime).OnComplete(() =>
            {
                if (_controller)
                {
                    foreach (var aura in Auras)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
                    }
                    _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage { Controller = _controller }, _controller.transform.parent.gameObject);
                }
            });
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_timer != null && _timer.IsActive())
            {
                _timer.Kill();
                _timer = null;
            }
        }
    }
}