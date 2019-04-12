using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    public class AnchorangProjectileAura : Aura
    {
        private GameObject _owner { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.Subscribe<SetOwnerMessage>(SetOwner);
            _controller.gameObject.SendMessageTo(new RequestOwnerMessage(), _controller.transform.parent.gameObject);
            if (_owner)
            {
                _controller.gameObject.SendMessageTo(new RegisterAnchorangMessage{AnchorangProjectile = _controller.transform.parent.gameObject}, _owner);
            }
            else
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            }
        }

        private void SetOwner(SetOwnerMessage msg)
        {
            _owner = msg.Owner;
        }
    }
}