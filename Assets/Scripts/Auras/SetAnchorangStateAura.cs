using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Set Anchorang State Aura", menuName = @"Anchorang/Aura/Anchorang/Set Anchorang State")]
    public class SetAnchorangStateAura : Aura
    {
        [Header("Set Anchorang State Aura settings")]
        public AnchorangState State;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new SetAnchorangStateMessage{State = State}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}