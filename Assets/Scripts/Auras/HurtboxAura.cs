using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Hurtbox Aura", menuName = @"Anchorang/Aura/Hurtbox")]
    public class HurtboxAura : Aura
    {
        public HurtboxController Hurtbox;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            if (Hurtbox)
            {
                Instantiate(Hurtbox, _controller.transform.parent);
            }
            else
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            }
        }
    }
}