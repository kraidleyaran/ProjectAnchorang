using Assets.Scripts.Animation;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Animation State Aura", menuName = @"Anchorang/Aura/Animation State")]
    public class AnimationStateAura : Aura
    {
        public UnitAnimationState State;

        public override void SubscribeController(AuraController controller)
        {
            if (DebugAura)
            {
                Debug.Break();
            }
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage{State = State}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}