using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Damage Aura", menuName = @"Anchorang/Aura/Damage", order = 0)]
    public class DamageAura : Aura
    {
        [Header("Damage Aura Settings")]
        public int Damage;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new TakeDamageMeessage{Damage = Damage}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }


    }
}