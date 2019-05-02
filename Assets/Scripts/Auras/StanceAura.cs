using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Stance Aura", menuName = @"Anchorang/Aura/Stance/Stance")]
    public class StanceAura : Aura
    {
        [Header("Stance Aura Settings")]
        public Color TunicColor;
        public int StaminaCost;
        public int StaminaPerSecond;
        public Sprite StanceSprite;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new SetAnchorangThrowCostMessage{Stamina = StaminaCost}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new SetStaminaRegenMessage{StaminaRegen = StaminaPerSecond}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessage(new UpdateStanceMessage{Stance = this});
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
}