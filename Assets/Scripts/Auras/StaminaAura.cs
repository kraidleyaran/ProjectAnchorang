using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Stamina Aura", menuName = @"Anchorang/Aura/Stats/Stamina")]
    public class StaminaAura : Aura
    {
        [Header("Stamina Aura Settings")]
        public int StartingStamina = 100;
        public int StartingMaximumStamina = 100;
        public int RegenThreshold = 100;

        private float _currentStamina { get; set; }
        private int _maximumStamina { get; set; }
        private int _staminaRegen { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _currentStamina = StartingStamina;
            _maximumStamina = StartingMaximumStamina;
            _controller.transform.parent.gameObject.SubscribeWithFilter<AdjustStaminaMessage>(AdjustStamina, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestStaminaMessage>(RequestStamina, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetStaminaRegenMessage>(SetStaminaRegen, _instanceId);
        }

        public override void Update()
        {
            base.Update();
            if (_currentStamina < RegenThreshold)
            {
                var addStamina = _staminaRegen * Time.deltaTime;
                _currentStamina += addStamina;
                if (_currentStamina > RegenThreshold)
                {
                    _currentStamina = RegenThreshold;
                }
                _controller.gameObject.SendMessageTo(new UpdateStaminaMessage { CurrentStamina = _currentStamina, MaximumStamina = _maximumStamina }, _controller.transform.parent.gameObject);
            }
            
            
        }


        private void AdjustStamina(AdjustStaminaMessage msg)
        {
            var adjustedValue = _currentStamina + msg.Stamina;
            if (adjustedValue >= 0 && adjustedValue <= _maximumStamina)
            {
                _currentStamina = adjustedValue;
            }
            _controller.gameObject.SendMessageTo(new UpdateStaminaMessage{CurrentStamina = _currentStamina, MaximumStamina = _maximumStamina}, _controller.transform.parent.gameObject);
            /*
            if (_regenTimer == null && _currentStamina < RegenThreshold)
            {
                StaminaRegen();
            }
            */
        }

        private void RequestStamina(RequestStaminaMessage msg)
        {
            _controller.gameObject.SendMessageTo(new UpdateStaminaMessage{CurrentStamina = _currentStamina, MaximumStamina = _maximumStamina}, msg.Sender);
        }

        private void SetStaminaRegen(SetStaminaRegenMessage msg)
        {
            _staminaRegen = msg.StaminaRegen;
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AdjustStaminaMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestStaminaMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetStaminaRegenMessage>(_instanceId);
        }
        
    }
}