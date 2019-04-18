using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "On Max Distance Aura", menuName = @"Anchorang/Aura/On Max Distance", order = 0)]
    public class OnMaxDistanceAura : Aura
    {
        public List<Aura> ApplyOnMaxDistance;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<MaxDistanceReachedMessage>(OnMaxDistance, _instanceId);
        }

        private void OnMaxDistance(MaxDistanceReachedMessage msg)
        {
            foreach (var aura in ApplyOnMaxDistance)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
            }
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<MaxDistanceReachedMessage>(_instanceId);
        }
    }
}