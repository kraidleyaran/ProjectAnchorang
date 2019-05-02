using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Prevent Input Aura", menuName = @"Anchorang/Aura/Input/Prevent Input", order = 0)]
    public class PreventInputAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.Subscribe<RequestInputStatusMessage>(RequestInputStatus);
            _controller.gameObject.SendMessage(new UpdateInputStatusMessage{AllowInput = false});
        }

        private void RequestInputStatus(RequestInputStatusMessage msg)
        {
            _controller.gameObject.SendMessageTo(new UpdateInputStatusMessage{AllowInput = false}, msg.Sender);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessage(new UpdateInputStatusMessage{AllowInput = true});
            _controller.gameObject.Unsubscribe<RequestInputStatusMessage>();
        }
    }
}