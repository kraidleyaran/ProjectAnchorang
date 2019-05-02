using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Adjust Stamina Aura", menuName = @"Anchorang/Aura/Stats/Adjust Stamina")]
    public class AdjustStaminaAura : Aura
    {
        public float Amount;
        public AdjustmentType AdjustmentType;

        private int _baseStamina { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            var amount = (int) Amount;
            if (AdjustmentType == AdjustmentType.Multiplier)
            {
                _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateStaminaMessage>(UpdateStamina, _instanceId);
                amount = _baseStamina * amount;
            }
            _controller.gameObject.SendMessageTo(new AdjustStaminaMessage{Stamina = amount}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }

        private void UpdateStamina(UpdateStaminaMessage msg)
        {
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateStaminaMessage>(_instanceId);
            _baseStamina = msg.MaximumStamina;
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateStaminaMessage>(_instanceId);
        }
    }
}