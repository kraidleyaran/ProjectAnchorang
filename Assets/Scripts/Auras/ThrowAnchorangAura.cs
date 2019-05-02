using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    //TODO: Add ability to have a class with methods that can be attached to a generic aura instead of creating these new ones everytime
    [CreateAssetMenu(fileName = "Throw Anchorang Aura", menuName = @"Anchorang/Aura/Anchorang/Throw Anchorang", order = 0)]
    public class ThrowAnchorangAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new ThrowAnchorangMessage(), _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}