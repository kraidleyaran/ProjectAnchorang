using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Set Player Hover Target Aura", menuName = @"Anchorang/Aura/Set Player Hover Target", order = 0)]
    public class SetPlayerHoverTargetAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessage(new SetPlayerHoverTargetMessage{Target = _controller.transform.parent.gameObject});
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}