using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Lock On Target Aura", menuName = @"Anchorang/Aura/Lock On Target", order = 0)]
    public class LockOnTargetAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new SetLockedTargetMessage(), _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessageTo(new SetHoverTargetMessage(), _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new SetLockedTargetMessage(), _controller.transform.parent.gameObject);
        }
    }
}