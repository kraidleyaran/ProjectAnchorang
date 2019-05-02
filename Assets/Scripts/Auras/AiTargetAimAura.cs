using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Ai Target Aim Aura", menuName = @"Anchorang/Aura/Ai/Ai Target Aim")]
    public class AiTargetAimAura : Aura
    {
        [Header("Ai Target Aim Aura Settings")]
        public bool TargetPlayer = true;

        private GameObject _target { get; set; }
        private Vector2 _aimDirection { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            if (TargetPlayer)
            {
                _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateplayerObjectMessage>(UpdatePlayerObject, _instanceId);
                _controller.transform.parent.gameObject.SendMessage(new RequestPlayerObjectMessage());
            }   
        }
        public override void Update()
        {
            if (_target)
            {
                _aimDirection = _target.transform.position - _controller.transform.position;
                _controller.gameObject.SendMessageTo(new UpdateAimDirectionMessage{Direction = _aimDirection}, _controller.transform.parent.gameObject);
            }
        }

        private void UpdatePlayerObject(UpdateplayerObjectMessage msg)
        {
            _target = msg.Player;
        }

        public override void Destroy()
        {
            base.Destroy();
            if (TargetPlayer)
            {
                _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateplayerObjectMessage>(_instanceId);
            }
            
        }
    }
}