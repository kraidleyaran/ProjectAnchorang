using System;
using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Movement Aura", menuName = @"Anchorang/Aura/Movement", order = 0)]
    public class MovementAura : Aura
    {
        [Header("Movement Aura Settings")]
        public MovementInfo MovementInfo;
        public PhysicsBoxController PhysicsBox;
        public bool AlwaysMoving;
        public List<Aura> StopMovementAuras;

        public override string AuraDescription
        {
            get
            {
                var currentAccelerationDelta = $"Current Acceleration Delta - {_currentAcceleration}";
                var currentAcceleration = $"Current Acceleration - {MovementInfo.Acceleration}";
                var currentMaxSpeed = $"Current Max Speed - {MovementInfo.MaxSpeed}";
                return $"{currentAccelerationDelta}{Environment.NewLine}{currentAcceleration}{Environment.NewLine}{currentMaxSpeed}";
            }
        }

        private Rigidbody2D _rigidBody { get; set; }
        private Vector2 _setDirection { get; set; }
        private float _currentAcceleration { get; set; }
        private float _currentDistance { get; set; }
        private Transform _target { get; set; }
        private Action _onDestinationReached { get; set; }
        private Tween _tweenedDestination { get; set; }

        public override void FixedUpdate()
        {
            if (_tweenedDestination == null)
            {
                if (_setDirection != Vector2.zero || _target)
                {
                    if (_target)
                    {
                        if (_rigidBody.OverlapPoint(_target.transform.position))
                        {
                            _controller.transform.gameObject.SendMessageTo(new ObjectHitMessage { ObjectHit = _target.gameObject }, _controller.transform.parent.gameObject);
                            return;
                        }
                        var distanceFromTarget = (_target.transform.position.ToVector2() - _rigidBody.position).magnitude;
                        _controller.gameObject.SendMessageTo(new DistanceFromTargetMessage { Distance = distanceFromTarget }, _controller.transform.parent.gameObject);

                        var direction = (_target.position.ToVector2() - _rigidBody.position).normalized;
                        _setDirection = direction;
                    }
                    if (MovementInfo.MaxDistance <= 0 || _currentDistance < MovementInfo.MaxDistance)
                    {
                        _currentAcceleration += MovementInfo.Acceleration;
                        if (_currentAcceleration > MovementInfo.MaxSpeed)
                        {
                            _currentAcceleration = MovementInfo.MaxSpeed;
                        }
                        var position = _rigidBody.position;
                        var acceleration = _currentAcceleration * Time.fixedDeltaTime;
                        if (acceleration > 0 || acceleration < 0)
                        {
                            position += acceleration * _setDirection;
                            if (MovementInfo.MaxDistance > 0)
                            {
                                _currentDistance += acceleration;
                            }
                            //position += Vector2.ClampMagnitude(_setDirection * acceleration,acceleration);
                            _rigidBody.MovePosition(position);
                            _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.Moving }, _controller.transform.parent.gameObject);

                        }

                    }
                    else
                    {
                        //_setDirection = Vector2.zero;
                        _controller.gameObject.SendMessageTo(new MaxDistanceReachedMessage(), _controller.transform.parent.gameObject);
                        //_controller.gameObject.SendMessageTo(new ObjectHitMessage(), _controller.transform.parent.gameObject);
                        //_controller.gameObject.SendMessageTo(new DestinationReachedMessage(), _controller.transform.parent.gameObject);
                    }
                }
                else if (_currentAcceleration > 0)
                {
                    if (!AlwaysMoving)
                    {
                        _currentAcceleration = 0;
                        _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.Idle }, _controller.transform.parent.gameObject);
                    }
                }
            }
        }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            if (PhysicsBox)
            {
                Instantiate(PhysicsBox, controller.transform.parent);
            }
            
            _rigidBody = controller.transform.parent.gameObject.GetComponent<Rigidbody2D>();
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetMovementDirectionMessage>(SetMovementDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestMovementInfoMessage>(RequestMovementInfo, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAccelerationMessage>(SetAcceleration, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetMaxSpeedMessage>(SetMaxSpeed, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<ResetMovementSpeedMessage>(ResetMovementSpeed, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetMaxDistanceMessage>(SetMaxDistance, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetTargetDestinationMessage>(SetTargetDestination, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<DestinationReachedMessage>(DestinationReached, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetPositionMessage>(SetPosition, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetRelativePositionMessage>(SetRelativePosition, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AdjustMovementValueMessage>(AdjustMovementValue, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetTweenedDestinationMessage>(SetTweenedDestination, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<FinishedTweendDestinationMessage>(FinishTweenedDestination, _instanceId);
        }

        private void SetMovementDirection(SetMovementDirectionMessage msg)
        {
            _setDirection = msg.Direction.normalized;
        }

        private void RequestMovementInfo(RequestMovementInfoMessage msg)
        {
            _controller.gameObject.SendMessageTo(new UpdateMovementInfoMessage { MovementInfo = MovementInfo}, msg.Sender);
        }

        private void SetAcceleration(SetAccelerationMessage msg)
        {
            MovementInfo.Acceleration = msg.Acceleration;
        }

        private void SetMaxSpeed(SetMaxSpeedMessage msg)
        {
            MovementInfo.MaxSpeed = msg.MaxSpeed;
        }

        private void ResetMovementSpeed(ResetMovementSpeedMessage msg)
        {
            _currentAcceleration = 0;
        }

        private void SetMaxDistance(SetMaxDistanceMessage msg)
        {
            MovementInfo.MaxDistance = msg.MaxDistance * Time.fixedDeltaTime;
            _currentDistance = 0;
        }

        private void SetTargetDestination(SetTargetDestinationMessage msg)
        {
            _target = msg.Target.transform;
            MovementInfo.MaxDistance = 0f;
            _currentDistance = 0f;
            _setDirection = Vector2.zero;
            _onDestinationReached = msg.OnDestinationReached;

        }

        private void DestinationReached(DestinationReachedMessage msg)
        {
            _onDestinationReached?.Invoke();
        }

        private void AdjustMovementValue(AdjustMovementValueMessage msg)
        {
            MovementInfo.SetValue(msg.MovementValueType, msg.Amount);
        }

        private void SetPosition(SetPositionMessage msg)
        {
            _rigidBody.MovePosition(msg.Position);
        }

        private void SetRelativePosition(SetRelativePositionMessage msg)
        {
            _rigidBody.MovePosition(_rigidBody.position + msg.RelativePosition);
        }

        private void SetTweenedDestination(SetTweenedDestinationMessage msg)
        {
            if (_tweenedDestination != null && _tweenedDestination.IsActive())
            {
                _tweenedDestination.Kill();
                _tweenedDestination = null;
            }
            _tweenedDestination = _rigidBody.DOMove(msg.Destination, msg.Time).SetEase(msg.Ease).SetSpeedBased(msg.SpeedBased).OnComplete(() =>
            {
                _controller.gameObject.SendMessageTo(new DestinationReachedMessage(), _controller.transform.parent.gameObject);
               msg.OnComplete?.Invoke();
                _tweenedDestination = null;
            });
        }

        private void FinishTweenedDestination(FinishedTweendDestinationMessage msg)
        {
            _tweenedDestination?.Kill(msg.Complete);
            _tweenedDestination = null;
        }

        public override void Destroy()
        {
            if (_tweenedDestination != null && _tweenedDestination.IsActive())
            {
                _tweenedDestination.Kill();
                _tweenedDestination = null;
            }
            //TODO: Change MessageBus so we can unsubscribe from all messages for the object with a given filter
            //TODO: Once the MessageBus change is done, move unsubscribe code into parent class then remove all destroy overrides that are only unsubscribing from filter
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetMovementDirectionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestMovementInfoMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetAccelerationMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetMaxSpeedMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ResetMovementSpeedMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetMaxDistanceMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetTargetDestinationMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<DestinationReachedMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetPositionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetRelativePositionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AdjustMovementValueMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetTweenedDestinationMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<FinishedTweendDestinationMessage>(_instanceId);
        }
    }
}