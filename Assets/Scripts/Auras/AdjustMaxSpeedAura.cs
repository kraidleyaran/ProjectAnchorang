using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Adjust Max Speed Aura", menuName = @"Anchorang/Aura/Adjust Max Speed")]
    public class AdjustMaxSpeedAura : Aura
    {
        public float Amount;
        public AdjustmentType AdjustmentType;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new AdjustMaxSpeedMessage{AdjustmentType = AdjustmentType, Amount = Amount}, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessageTo(new AdjustMaxSpeedMessage{AdjustmentType = AdjustmentType, Amount = Amount * -1}, _controller.transform.parent.gameObject);
        }
    }
}