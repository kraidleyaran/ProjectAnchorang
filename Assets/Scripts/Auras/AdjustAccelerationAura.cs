using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Adjust Acceleration Aura", menuName = @"Anchorang/Aura/Adjust Acceleration")]
    public class AdjustAccelerationAura : Aura
    {
        public float Amount;
        public AdjustmentType Type;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new AdjustAccelerationMessage{AdjustmentType = Type, Amount = Amount}, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessageTo(new AdjustAccelerationMessage{AdjustmentType = Type, Amount = Amount *-1}, _controller.transform.parent.gameObject);
        }
    }
}