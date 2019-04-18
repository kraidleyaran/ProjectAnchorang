using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Lock Animation State Aura", menuName = @"Anchorang/Aura/Lock Animation State")]
    public class LockAnimationStateAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage{Locked = true}, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage{Locked = false}, _controller.transform.parent.gameObject);
        }
    }
}