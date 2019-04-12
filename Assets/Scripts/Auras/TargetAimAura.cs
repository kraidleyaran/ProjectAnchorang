using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Target Aim Aura", menuName = @"Anchorang/Aura/Target Aim", order = 0)]
    public class TargetAimAura : Aura
    {
        [Header("Target Aim Aura Settings")]
        public List<Aura> TargetAuras;
        public OutlineAura HoverOutline;
        public OutlineAura LockedOnOutline;
        public float Distance;

        private Vector2 _aimDirection { get; set; }
        private GameObject _targetReticule { get; set; }
        private GameObject _hoverTarget { get; set; }
        private GameObject _lockedTarget { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AimDirectionMessage>(AimDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RegisterTargetAimReticuleMessage>(RegisterTargetAimObject, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetHoverTargetMessage>(SetHoverTarget, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetLockedTargetMessage>(SetLockedTarget, _instanceId);

            _controller.transform.parent.gameObject.SendMessageTo(new RequestAimDirectionMessage(), _controller.transform.parent.gameObject);
            var targetController = Instantiate(FactoryController.UNIT, _controller.transform.position, Quaternion.identity);
            foreach (var aura in TargetAuras)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, targetController.gameObject);
            }
            _controller.gameObject.SendMessageTo(new SetOwnerMessage{Owner = _controller.transform.parent.gameObject}, targetController.gameObject);
        }

        public override void Update()
        {
            if (_lockedTarget)
            {
                _controller.transform.parent.gameObject.SendMessageTo(new SetPositionMessage{Position = _lockedTarget.transform.position}, _targetReticule);
                _aimDirection = _lockedTarget.transform.position - _controller.transform.position;
                _controller.gameObject.SendMessageTo(new AimDirectionMessage{Direction = _aimDirection}, _controller.transform.parent.gameObject);
            }
        }

        private void AimDirection(AimDirectionMessage msg)
        {
            if (_targetReticule && !_lockedTarget)
            {
                _aimDirection = msg.Direction;
                _controller.gameObject.SendMessageTo(new SetPositionMessage{Position = _controller.transform.position.ToVector2() + _aimDirection.normalized * Distance}, _targetReticule);
            }
        }

        private void RegisterTargetAimObject(RegisterTargetAimReticuleMessage msg)
        {
            _targetReticule = msg.Object;
        }

        private void SetHoverTarget(SetHoverTargetMessage msg)
        {
            _hoverTarget = msg.Target;
        }

        private void SetLockedTarget(SetLockedTargetMessage msg)
        {
            if (_lockedTarget && !_hoverTarget)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraFromObjectMessage{Aura = LockedOnOutline}, _lockedTarget);
            }
            _lockedTarget = _hoverTarget;
            _hoverTarget = null;
            if (_lockedTarget)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = LockedOnOutline }, _lockedTarget);
            }
            else
            {
                _controller.gameObject.SendMessageTo(new SetPositionMessage{Position = _controller.transform.position.ToVector2() + _aimDirection.normalized * Distance }, _targetReticule);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AimDirectionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetHoverTargetMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetLockedTargetMessage>(_instanceId);
        }
    }
}