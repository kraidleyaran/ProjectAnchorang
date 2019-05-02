using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "On Demand Action Aura", menuName = @"Anchorang/Aura/On Demand/On Demand Action")]
    public class OnDemandActionAura : Aura
    {
        public List<Aura> AurasToApply;
        public string OnDemandEventName = "Default";

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<OnDemandEventMessage>(OnDemandEvent, _instanceId);
            
        }

        private void OnDemandEvent(OnDemandEventMessage msg)
        {
            if (msg.EventName == OnDemandEventName)
            {
                if (DebugAura)
                {
                    Debug.Break();
                }
                foreach (var aura in AurasToApply)
                {
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
                }
                _controller.transform.parent.gameObject.UnsubscribeFromFilter<OnDemandEventMessage>(_instanceId);
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<OnDemandEventMessage>(_instanceId);
        }
    }
}