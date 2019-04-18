using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Return to Owner Aura", menuName = @"Anchorang/Aura/Return to Owner")]
    public class ReturnToOwnerAura : Aura
    {
        [Header("Return to Owner Aura settings")]
        public List<Aura> OnReturnSelfAuras;
        public List<Aura> OnReturnOwnerAuras;

        private GameObject _owner { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetOwnerMessage>(SetOwner, _instanceId);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestOwnerMessage(), _controller.transform.parent.gameObject);
            if (_owner)
            {
                _controller.transform.parent.gameObject.SubscribeWithFilter<ObjectHitMessage>(ObjectHit, _instanceId);
                _controller.gameObject.SendMessageTo(new SetTargetDestinationMessage { Target = _owner }, _controller.transform.parent.gameObject);
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

        private void ObjectHit(ObjectHitMessage msg)
        {
            if (_owner && _owner == msg.ObjectHit)
            {
                foreach (var aura in OnReturnSelfAuras)
                {
                    _controller.transform.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
                }
                foreach (var aura in OnReturnOwnerAuras)
                {
                    _controller.transform.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _owner);
                }
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage { Controller = _controller }, _controller.transform.parent.gameObject);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetOwnerMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ObjectHitMessage>(_instanceId);
        }
    }
}