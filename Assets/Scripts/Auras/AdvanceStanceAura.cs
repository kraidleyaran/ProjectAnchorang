using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Advance Stance Aura", menuName = @"Anchorang/Aura/Stance/Advance Stance", order = 0)]
    public class AdvanceStanceAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new AdvanceStanceMessage(), _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}