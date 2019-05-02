using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Adjust Movement Value Aura", menuName = @"Anchorang/Aura/Stats/Adjust Movement Value")]
    public class AdjustMovementValueAura : Aura
    {
        [Header("Adjust Movement Value Aura Settings")]
        public float Amount;
        public AdjustmentType AdjustmentType;
        public MovementValueType MovementValueType;

        private float _appliedValue { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SubscribeWithFilter<UpdateMovementInfoMessage>(MovementInfo, _instanceId);
            _controller.gameObject.SendMessageTo(new RequestMovementInfoMessage(), _controller.transform.parent.gameObject);
        }

        private void MovementInfo(UpdateMovementInfoMessage msg)
        {
            _controller.gameObject.UnsubscribeFromFilter<UpdateMovementInfoMessage>(_instanceId);
            var sendValue = Amount;
            if (AdjustmentType == AdjustmentType.Multiplier)
            {
                var baseValue = msg.MovementInfo.GetValue(MovementValueType);
                sendValue = Amount * baseValue;
            }
            _appliedValue = sendValue;
            _controller.gameObject.SendMessageTo(new AdjustMovementValueMessage{MovementValueType = MovementValueType, Amount = sendValue}, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessageTo(new AdjustMovementValueMessage{MovementValueType = MovementValueType, Amount = _appliedValue * -1}, _controller.transform.parent.gameObject);
        }
    }
}