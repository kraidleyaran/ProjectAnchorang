using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Move Camera Aura", menuName = @"Anchorang/Aura/Move Camera")]
    public class MoveCameraAura : Aura
    {
        private Rigidbody2D _rigidbody { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _rigidbody = _controller.transform.parent.GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdate()
        {
            _controller.gameObject.SendMessage(new MoveCameraToMessage{Position = _rigidbody.position});
        }
    }
}