using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Remove Aura Cooldown Aura", menuName = @"Anchorang/Aura/Remove Aura Cooldown")]
    public class RemoveAuraCooldownAura : Aura
    {
        public Aura Aura;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new RemoveAuraCooldownMessage{Aura = Aura}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}