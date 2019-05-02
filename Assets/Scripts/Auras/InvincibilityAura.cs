using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Invincibility Aura", menuName = @"Anchorang/Aura/Invincibility")]
    public class InvincibilityAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new InvincibilityStatusMessage{IsInvincible = true}, _controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.SubscribeWithFilter<InvincibilityCheckMessage>(InvincibilityCheck, _instanceId);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessageTo(new InvincibilityStatusMessage{IsInvincible = false}, _controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<InvincibilityCheckMessage>(_instanceId);
        }

        private void InvincibilityCheck(InvincibilityCheckMessage msg)
        {
            msg.DoAfter?.Invoke();
        }
    }

}