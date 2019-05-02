using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Animation Check", menuName = @"Anchorang/Aura/Animation Check")]
    public class AnimationCheckAura : Aura
    {
        public List<UnitAnimationState> AnimationStates;
        public bool ApplyIf;
        public List<Aura> AurasToApply;

        public override void SubscribeController(AuraController controller)
        {
            if (DebugAura)
            {
                Debug.Break();
            }
            base.SubscribeController(controller);
            var apply = !ApplyIf;
            _controller.gameObject.SendMessageTo(new AnimationCheckMessage{States = AnimationStates, DoAfter = () =>
            {
                if (!ApplyIf)
                {
                    apply = false;
                }
            }}, _controller.transform.parent.gameObject);
            
            if (apply)
            {
                foreach (var aura in AurasToApply)
                {
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
                }
            }
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}