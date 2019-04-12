using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Apply Aura Cooldown Aura", menuName = @"Anchorang/Aura/Apply Aura Cooldown")]
    public class ApplyAuraCooldownAura : Aura
    {
        [Header("Apply Aura Cooldown Settings")]
        public Aura Aura;
        public float Cooldown;
        public bool ReApplyOffCooldown;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new AddAuraCooldownMessage{Aura = Aura, Cooldown = Cooldown, ApplyAfterCooldown = ReApplyOffCooldown}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}