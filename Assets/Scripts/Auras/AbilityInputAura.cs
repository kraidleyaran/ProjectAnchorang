using Assets.Scripts.Abilities;
using Assets.Scripts.System;
using Assets.Scripts.System.Input;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Ability Input Aura", menuName = @"Anchorang/Aura/Ability Input")]
    public class AbilityInputAura : Aura
    {
        [Header("Ability Input Aura Settings")]
        public GameInputButton Button;
        public Ability Ability;

        private bool _preventInput { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            //_controller.gameObject.SendMessage(new RegisterObjectForInputMessage{Object = _controller.transform.parent.gameObject});
            _controller.gameObject.Subscribe<InputStateMessage>(InputState);
            _controller.gameObject.Subscribe<UpdateInputStatusMessage>(UpdateInputStatus);
            _controller.gameObject.SendMessage(new RequestInputStatusMessage());
        }

        private void InputState(InputStateMessage msg)
        {
            if (!msg.PreviousState.Buttons.Contains(Button) && msg.CurrentState.Buttons.Contains(Button))
            {
                _controller.gameObject.SendMessage(new RequestInputStatusMessage());
                if (!_preventInput)
                {
                    if (DebugAura)
                    {
                        Debug.Break();
                        Debug.Log($"{Ability.Name} Ability used");
                    }
                    Ability.UseAbility(_controller.transform.parent.gameObject);
                }
            }
        }

        private void UpdateInputStatus(UpdateInputStatusMessage msg)
        {
            _preventInput = !msg.AllowInput;
        }

        public override void Destroy()
        {
            base.Destroy();
            //_controller.gameObject.UnsubscribeFromFilter<InputStateMessage>(_instanceId);
            //_controller.gameObject.SendMessage(new UnregisterObjectForInputMessage{Object = _controller.transform.parent.gameObject});
        }
    }
}