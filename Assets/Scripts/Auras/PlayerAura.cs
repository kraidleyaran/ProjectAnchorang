using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Player Aura", menuName = @"Anchorang/Aura/Player/Player")]
    public class PlayerAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.Subscribe<RequestPlayerObjectMessage>(RequestPlayerObject);
        }

        private void RequestPlayerObject(RequestPlayerObjectMessage msg)
        {
            _controller.gameObject.SendMessageTo(new UpdateplayerObjectMessage{Player = _controller.transform.parent.gameObject}, msg.Sender);
        }
    }
}