using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Ownership Aura", menuName = @"Anchorang/Aura/Ownership")]
    public class OwnershipAura : Aura
    {
        private GameObject _owner { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetOwnerMessage>(SetOwner, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestOwnerMessage>(RequestOwner, _instanceId);
        }

        private void SetOwner(SetOwnerMessage msg)
        {
            _owner = msg.Owner;
        }

        private void RequestOwner(RequestOwnerMessage msg)
        {
            _controller.gameObject.SendMessageTo(new SetOwnerMessage{Owner = _owner}, msg.Sender);
        }

        public override void Destroy()
        {
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetOwnerMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestOwnerMessage>(_instanceId);
        }
    }
}