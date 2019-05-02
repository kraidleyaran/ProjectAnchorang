using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Animation;
using Assets.Scripts.System;
using Assets.Scripts.System.Input;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Dash Aura", menuName = @"Anchorang/Aura/Dash", order = 0)]
    public class DashAura : Aura
    {
        private const string DASH_EVENT = "Dash";
        private const string FINISH_DASH_EVENT = "Finish Dash";
        private const string SPIN_SLASH = "Spin Slash";
        private const string COMPLETE_SPIN_SLASH = "Complete Spin Slash";

        [Header("Dash Aura Settings")]
        public List<Aura> DashAuras;
        public List<Aura> SpinSlashAuras;
        public float MovingDashDistance;
        public float IdleDashDistance;
        public float DashTime = 1f;
        
        public GameInputButton Input;

        private Vector2 _moveDirection { get; set; }
        private Vector2 _aimDirection { get; set; }
        private bool _isDashing { get; set; }
        private int _staminaCost { get; set; }
        private float _currentStamina { get; set; }
        private GameObject _dashDestinationObject { get; set; }
        private bool _preventInput { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetMovementDirectionMessage>(SetMovementDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAimDirectionMessage>(SetAimDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetDashStateMessage>(SetDashState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateStaminaMessage>(UpdateStamina, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetDashStaminaCostMessage>(SetDashStaminaCost, _instanceId);
            _controller.gameObject.Subscribe<InputStateMessage>(InputState);
            _controller.gameObject.Subscribe<UpdateInputStatusMessage>(UpdateInputStatus);
            _controller.gameObject.SendMessageTo(new RequestStaminaMessage(), _controller.transform.parent.gameObject);
        }

        private void FinishDash()
        {
            _controller.gameObject.SendMessageTo(new FinishedTweendDestinationMessage{Complete = false}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage { Locked = false }, _controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<DestinationReachedMessage>(_instanceId);
            Destroy(_dashDestinationObject);
            _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.DashComplete }, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage { Locked = true }, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessage(new UpdateInputStatusMessage{AllowInput = false});
        }

        private void CompletedDash()
        {
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<OnDemandEventMessage>(_instanceId);
            foreach (var aura in DashAuras)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraFromObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
            }
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage{Locked = false}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.Idle }, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessage(new UpdateInputStatusMessage{AllowInput = true});
            _isDashing = false;
        }

        private void SpinSlash()
        {
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<OnDemandEventMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<DestinationReachedMessage>(_instanceId);
            _controller.transform.parent.gameObject.SendMessageTo(new FinishedTweendDestinationMessage{Complete = false}, _controller.transform.parent.gameObject);
            foreach (var aura in SpinSlashAuras)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
            }
            _controller.transform.parent.gameObject.SubscribeWithFilter<OnDemandEventMessage>(OnDemandEvent, _instanceId);

            //_isDashing = false;
        }

        private void CompleteSpinSlash()
        {
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<OnDemandEventMessage>(_instanceId);
            var auras = DashAuras.ToList();
            auras.AddRange(SpinSlashAuras);
            foreach (var aura in auras)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraFromObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
            }
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage { Locked = false }, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.Idle }, _controller.transform.parent.gameObject);
            //_controller.gameObject.SendMessage(new UpdateInputStatusMessage { AllowInput = true });
            _isDashing = false;
        }

        private void SetMovementDirection(SetMovementDirectionMessage msg)
        {
            _moveDirection = msg.Direction;
        }

        private void SetAimDirection(SetAimDirectionMessage msg)
        {
            _aimDirection = msg.Direction;
        }

        private void SetDashState(SetDashStateMessage msg)
        {
            //TODO: Remove this message and function as we're working with animations to assure this
            _isDashing = msg.IsDashing;
        }

        private void InputState(InputStateMessage msg)
        {
            var proceed = true;
            _controller.gameObject.SendMessageTo(new AuraCooldownCheckMessage{Predicate = a => a.Name == Name, Action =
                auras =>
                {
                    proceed = auras.Count <= 0;
                }}, _controller.transform.parent.gameObject);
            if (proceed && !_isDashing && _currentStamina >= _staminaCost && !msg.PreviousState.Buttons.Contains(Input) && msg.CurrentState.Buttons.Contains(Input))
            {
                _controller.gameObject.SendMessage(new RequestInputStatusMessage());
                if (!_preventInput)
                {
                    foreach (var aura in DashAuras)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
                    }
                    var dashDestination = _controller.transform.parent.position.ToVector2();
                    if (_moveDirection != Vector2.zero)
                    {
                        dashDestination += _moveDirection * MovingDashDistance;
                    }
                    else
                    {
                        dashDestination += _aimDirection * IdleDashDistance;
                    }
                    _dashDestinationObject = Instantiate(FactoryController.DESTINATION, dashDestination, Quaternion.identity);
                    //_controller.gameObject.SendMessageTo(new SetTargetDestinationMessage{Target = _dashDestinationObject }, _controller.transform.parent.gameObject );
                    //_controller.transform.parent.gameObject.SubscribeWithFilter<ObjectHitMessage>(ObjectHit, _instanceId);
                    _controller.gameObject.SendMessageTo(new SetTweenedDestinationMessage { Destination = _dashDestinationObject.transform.position, Time = DashTime, Ease = Ease.Linear }, _controller.transform.parent.gameObject);
                    _controller.transform.parent.gameObject.SubscribeWithFilter<DestinationReachedMessage>(DestinationReached, _instanceId);
                    _controller.gameObject.SendMessageTo(new AdjustStaminaMessage { Stamina = _staminaCost * -1 }, _controller.transform.parent.gameObject);
                    _controller.transform.parent.gameObject.SubscribeWithFilter<OnDemandEventMessage>(OnDemandEvent, _instanceId);
                    _controller.gameObject.SendMessage(new UpdateDashCooldownIconMessage{Time = 1.5f});
                    _isDashing = true;
                }
            }
        }

        private void OnDemandEvent(OnDemandEventMessage msg)
        {
            switch (msg.EventName)
            {
                case DASH_EVENT:
                    CompletedDash();
                    break;
                case FINISH_DASH_EVENT:
                    FinishDash();
                    break;
                case SPIN_SLASH:
                    SpinSlash();
                    break;
                case COMPLETE_SPIN_SLASH:
                    CompleteSpinSlash();
                    break;
            }
        }

        private void UpdateStamina(UpdateStaminaMessage msg)
        {
            _currentStamina = msg.CurrentStamina;
        }

        private void SetDashStaminaCost(SetDashStaminaCostMessage msg)
        {
            _staminaCost = msg.StaminaCost;
        }

        private void DestinationReached(DestinationReachedMessage msg)
        {
            FinishDash();
            /*
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage{Locked = false}, _controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<DestinationReachedMessage>(_instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<OnDemandEventMessage>(OnDemandEvent, _instanceId);
            Destroy(_dashDestinationObject);
            _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage{State = UnitAnimationState.DashComplete}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new LockAnimationStateMessage { Locked = true }, _controller.transform.parent.gameObject);
            */
        }

        private void ObjectHit(ObjectHitMessage msg)
        {
            if (msg.ObjectHit == _dashDestinationObject)
            {
                _controller.gameObject.SendMessageTo(new LockAnimationStateMessage { Locked = false }, _controller.transform.parent.gameObject);
                _controller.transform.parent.gameObject.UnsubscribeFromFilter<ObjectHitMessage>(_instanceId);
                _controller.transform.parent.gameObject.SubscribeWithFilter<OnDemandEventMessage>(OnDemandEvent, _instanceId);
                Destroy(_dashDestinationObject);
                _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.DashComplete }, _controller.transform.parent.gameObject);
                _controller.gameObject.SendMessageTo(new LockAnimationStateMessage { Locked = true }, _controller.transform.parent.gameObject);
                _dashDestinationObject = null;
            }
        }

        private void UpdateInputStatus(UpdateInputStatusMessage msg)
        {
            _preventInput = !msg.AllowInput;
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetMovementDirectionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetAimDirectionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<OnDemandEventMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetDashStaminaCostMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetDashStateMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<DestinationReachedMessage>(_instanceId);
        }
    }
}