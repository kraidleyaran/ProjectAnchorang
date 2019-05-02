using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Stance State Aura", menuName = @"Anchorang/Aura/Stance/Stance State")]
    public class StanceStateAura : Aura
    {
        [Header("Stance State Aura Settings")]
        public List<StanceAura> AvailableStances;
        public List<Aura> ApplyOnStanceChange;

        private StanceAura _currentStance { get; set; }
        private float _stamina { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetCurrentStanceMessage>(SetCurrentStance, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateStaminaMessage>(UpdateStamina, _instanceId);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestStaminaMessage(), _controller.transform.parent.gameObject);

            if (AvailableStances.Count > 0)
            {
                var startingStance = AvailableStances[0];
                _currentStance = startingStance;
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = _currentStance }, _controller.transform.parent.gameObject);
                _controller.gameObject.SendMessageTo(new SetOutlineMessage { Color = _currentStance.TunicColor, Thickness = 1}, _controller.transform.parent.gameObject);
                _controller.transform.parent.gameObject.SubscribeWithFilter<AdvanceStanceMessage>(AdvanceStance, _instanceId);
            }
            
        }

        private void SetCurrentStance(SetCurrentStanceMessage msg)
        {
            if (_currentStance)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraFromObjectMessage { Aura = _currentStance }, _controller.transform.parent.gameObject);
            }
            _currentStance = msg.Stance;
            _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = _currentStance }, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new SetOutlineMessage { Color = msg.Stance.TunicColor, Thickness = 1 }, _controller.transform.parent.gameObject);
            foreach (var aura in ApplyOnStanceChange)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
            }
        }

        private void AdvanceStance(AdvanceStanceMessage msg)
        {
            if (_currentStance)
            {
                var childIndex = AvailableStances.FindIndex(s => s == _currentStance);
                if (childIndex < AvailableStances.Count - 1)
                {
                    childIndex++;
                }
                else
                {
                    childIndex = 0;
                }
                var stance = AvailableStances[childIndex];
                _controller.gameObject.SendMessageTo(new SetCurrentStanceMessage{Stance = stance}, _controller.transform.parent.gameObject);
            }
        }

        private void UpdateStamina(UpdateStaminaMessage msg)
        {
            _stamina = msg.CurrentStamina;
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_currentStance)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraFromObjectMessage{Aura = _currentStance}, _controller.transform.parent.gameObject);
            }
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetCurrentStanceMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AdvanceStanceMessage>(_instanceId);
        }
    }
}