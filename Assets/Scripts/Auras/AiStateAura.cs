using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Ai State Aura", menuName = @"Anchorang/Aura/Ai/Ai State")]
    public class AiStateAura : Aura
    {
        private AiState _state { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAiStateMessage>(SetAiState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestAiStateMessage>(RequestAiState, _instanceId);
        }

        private void SetAiState(SetAiStateMessage msg)
        {
            _state = msg.State;
            _controller.gameObject.SendMessageTo(new UpdateAiStateMessage{State = _state}, _controller.transform.parent.gameObject);
        }

        private void RequestAiState(RequestAiStateMessage msg)
        {
            _controller.gameObject.SendMessageTo(new UpdateAiStateMessage{State = _state}, msg.Sender);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetAiStateMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestAiStateMessage>(_instanceId);
        }
    }
}