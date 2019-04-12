using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Aim Aura", menuName = @"Anchorang/Aura/Aim", order = 0)]
    public class AimAura : Aura
    {
        private Vector2 _aimDirection { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _aimDirection = Vector2.up;
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAimDirectionMessage>(SetAimDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestAimDirectionMessage>(RequestAimDirection, _instanceId);
        }

        private void SetAimDirection(SetAimDirectionMessage msg)
        {
            _aimDirection = msg.Direction;
            _controller.gameObject.SendMessageTo(new AimDirectionMessage{Direction = _aimDirection}, _controller.transform.parent.gameObject);
        }

        private void RequestAimDirection(RequestAimDirectionMessage msg)
        {
            _controller.gameObject.SendMessageTo(new AimDirectionMessage{Direction = _aimDirection}, msg.Sender);
        }

        public override void Destroy()
        {
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetAimDirectionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestAimDirectionMessage>(_instanceId);
        }
    }
}