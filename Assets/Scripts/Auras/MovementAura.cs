using System;
using Assets.Scripts.Animation;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Movement Aura", menuName = @"Anchorang/Aura/Movement", order = 0)]
    public class MovementAura : Aura
    {
        [Header("Movement Aura Settings")]
        public float BaseAcceleration;
        public float BaseMaxSpeed;
        public float MaxDistance;
        public PhysicsBoxController PhysicsBox;


        private float _acceleration { get; set; }
        private float _maxSpeed { get; set; }

        private Rigidbody2D _rigidBody { get; set; }
        //private Vector2 _movingDirection { get; set; }
        private Vector2 _setDirection { get; set; }
        private float _currentAcceleration { get; set; }
        private float? _maxDistance { get; set; }
        private float _currentDistance { get; set; }
        private Transform _target { get; set; }
        private Action _onDestinationReached { get; set; }

        public override void FixedUpdate()
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
                    var direction = (_target.position.ToVector2() - _rigidBody.position).normalized;
                    _setDirection = direction;
                }
                if (!_maxDistance.HasValue || _currentDistance < _maxDistance)
                {
                    _currentAcceleration += _acceleration;
                    if (_currentAcceleration > _maxSpeed)
                    {
                        _currentAcceleration = _maxSpeed;
                    }
                    var position = _rigidBody.position;
                    var acceleration = _currentAcceleration * Time.fixedDeltaTime;
                    //position += Vector2.ClampMagnitude(_setDirection * acceleration,acceleration);
                    position += acceleration * _setDirection;
                    if (_maxDistance.HasValue)
                    {
                        _currentDistance += acceleration;
                    }
                    _rigidBody.MovePosition(position);
                    _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.Moving }, _controller.transform.parent.gameObject);
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
                _currentAcceleration = 0;
                _controller.gameObject.SendMessageTo(new SetUnitAnimationStateMessage { State = UnitAnimationState.Idle }, _controller.transform.parent.gameObject);
            }
        }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            if (PhysicsBox)
            {
                Instantiate(PhysicsBox, controller.transform.parent);
            }
            _acceleration = BaseAcceleration;
            _maxSpeed = BaseMaxSpeed;
            _rigidBody = controller.transform.parent.gameObject.GetComponent<Rigidbody2D>();
            if (MaxDistance > 0)
            {
                _maxDistance = MaxDistance * Time.deltaTime;
            }
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetMovementDirectionMessage>(SetMovementDirection, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestMovementInfoMessage>(RequestMovementInfo, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAccelerationMessage>(SetAcceleration, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetMaxSpeedMessage>(SetMaxSpeed, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<ResetMovementSpeedMessage>(ResetMovementSpeed, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetMaxDistanceMessage>(SetMaxDistance, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetTargetDestinationMessage>(SetTargetDestination, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<DestinationReachedMessage>(DestinationReached, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AdjustAccelerationMessage>(AdjustAcceleration, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AdjustMaxSpeedMessage>(AdjustMaxSpeed, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetPositionMessage>(SetPosition, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetRelativePositionMessage>(SetRelativePosition, _instanceId);
        }

        private void SetMovementDirection(SetMovementDirectionMessage msg)
        {
            _setDirection = msg.Direction.normalized;
        }

        private void RequestMovementInfo(RequestMovementInfoMessage msg)
        {
            _controller.gameObject.SendMessageTo(new MovementInfoMessage { Accleration = _acceleration, MaxSpeed = _maxSpeed }, msg.Sender);
        }

        private void SetAcceleration(SetAccelerationMessage msg)
        {
            _acceleration = msg.Acceleration;
        }

        private void SetMaxSpeed(SetMaxSpeedMessage msg)
        {
            _maxSpeed = msg.MaxSpeed;
        }

        private void ResetMovementSpeed(ResetMovementSpeedMessage msg)
        {
            _currentAcceleration = 0;
        }

        private void SetMaxDistance(SetMaxDistanceMessage msg)
        {
            _maxDistance = msg.MaxDistance * Time.fixedDeltaTime;
            _currentDistance = 0;
        }

        private void SetTargetDestination(SetTargetDestinationMessage msg)
        {
            _target = msg.Target.transform;
            _maxDistance = null;
            _currentDistance = 0f;
            _setDirection = Vector2.zero;
            _onDestinationReached = msg.OnDestinationReached;

        }

        private void DestinationReached(DestinationReachedMessage msg)
        {
            _onDestinationReached?.Invoke();
        }

        private void AdjustAcceleration(AdjustAccelerationMessage msg)
        {
            var amount = msg.Amount;
            if (msg.AdjustmentType == AdjustmentType.Multiplier)
            {
                amount = _acceleration + (_acceleration * msg.Amount);
            }
            _acceleration += amount;
        }

        private void AdjustMaxSpeed(AdjustMaxSpeedMessage msg)
        {
            var amount = msg.Amount;
            if (msg.AdjustmentType == AdjustmentType.Multiplier)
            {
                amount = _maxSpeed + (_maxSpeed * msg.Amount);
            }
            _maxSpeed += amount;
        }

        private void SetPosition(SetPositionMessage msg)
        {
            _rigidBody.MovePosition(msg.Position);
        }

        private void SetRelativePosition(SetRelativePositionMessage msg)
        {
            _rigidBody.MovePosition(_rigidBody.position + msg.RelativePosition);
        }

        public override void Destroy()
        {
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
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AdjustAccelerationMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AdjustMaxSpeedMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetPositionMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetRelativePositionMessage>(_instanceId);
        }
    }
}