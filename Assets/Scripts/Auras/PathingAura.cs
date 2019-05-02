using System.Collections.Generic;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Pathing Aura", menuName = @"Anchorang/Aura/Pathing", order = 0)]
    public class PathingAura : Aura
    {
        private List<Vector2> _pathingList { get; set; }
        private AiState _aiState { get; set; }
        private MovementInfo _movementInfo { get; set; }
        private int _currentChildIndex { get; set; }
        private GameObject _positionObject { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _pathingList = new List<Vector2>();
            _currentChildIndex = 0;
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetPathingMessage>(SetPathing, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateAiStateMessage>(UpdateAiState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateMovementInfoMessage>(UpdateMovementInfo, _instanceId);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestAiStateMessage(), _controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestMovementInfoMessage(), _controller.transform.parent.gameObject);

        }

        private void PathToNextPosition()
        {
            if (_pathingList.Count > 0)
            {
                if (_currentChildIndex > _pathingList.Count - 1)
                {
                    _currentChildIndex = 0;
                }
                _positionObject = Instantiate(FactoryController.DESTINATION, _pathingList[_currentChildIndex],Quaternion.identity);
                _controller.transform.parent.gameObject.SubscribeWithFilter<ObjectHitMessage>(ObjectHit, _instanceId);
                //_controller.transform.parent.gameObject.SubscribeWithFilter<DestinationReachedMessage>(DestinationReached, _instanceId);
                _controller.gameObject.SendMessageTo(new SetTargetDestinationMessage{Target = _positionObject}, _controller.transform.parent.gameObject);
            }
        }

        private void SetPathing(SetPathingMessage msg)
        {
            _pathingList = msg.Positions;
            _controller.gameObject.SendMessageTo(new FinishedTweendDestinationMessage{Complete = false}, _controller.transform.parent.gameObject);
            PathToNextPosition();
        }

        private void DestinationReached(DestinationReachedMessage msg)
        {
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<DestinationReachedMessage>(_instanceId);

        }

        private void ObjectHit(ObjectHitMessage msg)
        {
            if (msg.ObjectHit == _positionObject)
            {
                _currentChildIndex++;
                Destroy(_positionObject);
                _positionObject = null;
                PathToNextPosition();
            }
        }

        private void UpdateAiState(UpdateAiStateMessage msg)
        {
            _aiState = msg.State;
        }

        private void UpdateMovementInfo(UpdateMovementInfoMessage msg)
        {
            _movementInfo = msg.MovementInfo;
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetPathingMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateAiStateMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateMovementInfoMessage>(_instanceId);
            //_controller.transform.parent.gameObject.UnsubscribeFromFilter<DestinationReachedMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ObjectHitMessage>(_instanceId);
        }
    }
}