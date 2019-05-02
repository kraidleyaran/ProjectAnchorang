using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Activate On Demand Event Aura", menuName = @"Anchorang/Aura/On Demand/Activate On Demand Event", order = 0)]
    public class ActivateOnDemandEventAura : Aura
    {
        [Header("Activate On Demand Event Aura Settings")]
        public string EventName;
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new OnDemandEventMessage{EventName = EventName}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}