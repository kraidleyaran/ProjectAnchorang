using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Target Reticle Aura", menuName = @"Anchorang/Aura/Target Reticle", order = 0)]
    public class TargetReticleAura : Aura
    {
        public List<Aura> TargetableAuras;

        private GameObject _owner { get; set; }
        private GameObject _target { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetOwnerMessage>(SetOwner, _instanceId);
        }

        private void SetOwner(SetOwnerMessage msg)
        {
            _owner = msg.Owner;
            //_controller.transform.parent.gameObject.SubscribeWithFilter<ObjectHitMessage>(ObjectHit);
            _controller.transform.parent.gameObject.SendMessageTo(new RegisterTargetAimReticuleMessage{Object = _controller.transform.parent.gameObject}, msg.Owner);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetHoverTargetMessage>(SetHoverTarget, _instanceId);
            //_controller.transform.parent.gameObject.SubscribeWithFilter<ObjectLeftMessage>(ObjectLeft);
            _controller.gameObject.Subscribe<SetPlayerHoverTargetMessage>(SetPlayerHoverTarget);
        }

        private void ObjectHit(ObjectHitMessage msg)
        {
            if (msg.ObjectHit != _owner)
            {
                var setHover = false;
                _controller.gameObject.SendMessageTo(new AuraCheckMessage
                {
                    Predicate = aura => TargetableAuras.Exists(a => a.Name == aura.Name),
                    DoAfter =
                        auras =>
                        {
                            setHover = auras.Count > 0;
                        }
                }, msg.ObjectHit);
                if (setHover)
                {
                    _controller.gameObject.SendMessageTo(new SetHoverTargetMessage { Target = msg.ObjectHit }, _owner);
                }
            }
        }

        private void ObjectLeft(ObjectLeftMessage msg)
        {
            if (msg.Object != _owner && _target == msg.Object)
            {
                _target = null;
            }
        }

        private void SetHoverTarget(SetHoverTargetMessage msg)
        {
            _target = msg.Target;
        }

        private void SetPlayerHoverTarget(SetPlayerHoverTargetMessage msg)
        {
            _controller.gameObject.SendMessageTo(new SetHoverTargetMessage{Target = msg.Target}, _owner);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetOwnerMessage>(_instanceId);
        }
    }
}