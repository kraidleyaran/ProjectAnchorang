using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Apply To Owner", menuName = @"Anchorang/Aura/Apply To Owner")]
    public class ApplyToOwnerAura : Aura
    {
        public List<Aura> AurasToApply;

        private GameObject _owner { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetOwnerMessage>(SetOwner, _instanceId);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestOwnerMessage(), _controller.transform.parent.gameObject);
        }

        private void SetOwner(SetOwnerMessage msg)
        {
            _owner = msg.Owner;
            foreach (var aura in AurasToApply)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _owner);
            }
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage { Controller = _controller }, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetOwnerMessage>(_instanceId);
        }
    }
}