using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "On Distance From Target Aura", menuName = @"Anchorang/Aura/On Distance From Target")]
    public class OnDistanceFromTargetAura : Aura
    {
        public float DetectDistance = 0f;
        public List<Aura> AurasToApply;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<DistanceFromTargetMessage>(DistanceFromTarget, _instanceId);
        }

        private void DistanceFromTarget(DistanceFromTargetMessage msg)
        {
            if (msg.Distance <= DetectDistance)
            {
                foreach (var aura in AurasToApply)
                {
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
                }
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<DistanceFromTargetMessage>(_instanceId);
        }
    }
}