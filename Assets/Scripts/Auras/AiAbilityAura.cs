using System.Collections.Generic;
using Assets.Scripts.Abilities;
using Assets.Scripts.System;
using DG.Tweening;
using JetBrains.Annotations;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Ai Ability", menuName = @"Anchorang/Aura/Ai/Ai Ability")]
    public class AiAbilityAura : Aura
    {
        public List<Aura> Auras;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            foreach (var aura in Auras)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
            }
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}