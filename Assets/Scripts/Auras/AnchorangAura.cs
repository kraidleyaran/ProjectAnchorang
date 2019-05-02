using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Anchorang Aura", menuName = @"Anchorang/Aura/Anchorang")]
    public class AnchorangAura : Aura
    {

        [Header("Anchorang Aura Settings")]
        public int MaxAnchorangs = 1;
        public AbilityInputAura ThrowAnchorangInput;
        public List<Aura> AnchorangAuras;
        public int EquippedDashCost = 0;
        public int UnequippedDashCost = 0;
        
        private int _currentStaminaCost { get; set; }
        private float _currentStamina { get; set; }
        private AnchorangState _state { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateStaminaMessage>(UpdateStamina, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAnchorangStateMessage>(SetAnchorangState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<ThrowAnchorangMessage>(ThrowAnchorang, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAnchorangThrowCostMessage>(SetAnchorangThrowCost, _instanceId);
            _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = ThrowAnchorangInput}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new SetDashStaminaCostMessage{StaminaCost = EquippedDashCost}, _controller.transform.parent.gameObject);
        }

        private void UpdateStamina(UpdateStaminaMessage msg)
        {
            _currentStamina = msg.CurrentStamina;
            var hasEnoughStamina = _currentStamina >= _currentStaminaCost;
            _controller.gameObject.SendMessage(new UpdateRequiredStaminamStatusMessage{HasEnough = hasEnoughStamina});
        }

        private void SetAnchorangState(SetAnchorangStateMessage msg)
        {
            _state = msg.State;
            var stamina = 0;
            switch (_state)
            {
                case AnchorangState.Caught:
                    stamina = EquippedDashCost;
                    break;
                case AnchorangState.Thrown:
                    stamina = UnequippedDashCost;
                    break;
            }
            _controller.gameObject.SendMessageTo(new SetDashStaminaCostMessage{StaminaCost = stamina},  _controller.transform.parent.gameObject);
        }

        private void ThrowAnchorang(ThrowAnchorangMessage msg)
        {
            _controller.gameObject.SendMessageTo(new AuraCheckMessage{Predicate = a => a.Name == ThrowAnchorangInput.Name, DoAfter =
                auras =>
                {
                    if (auras.Count > 0 && _currentStamina >= _currentStaminaCost)
                    {
                        _controller.gameObject.SendMessageTo(new AdjustStaminaMessage { Stamina = _currentStaminaCost * -1 }, _controller.transform.parent.gameObject);
                        foreach (var aura in AnchorangAuras)
                        {
                            _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
                        }
                    }
                }}, _controller.transform.parent.gameObject);
        }

        private void SetAnchorangThrowCost(SetAnchorangThrowCostMessage msg)
        {
            _currentStaminaCost = msg.Stamina;
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateStaminaMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetAnchorangStateMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ThrowAnchorangMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetAnchorangThrowCostMessage>(_instanceId);
        }
    }
}