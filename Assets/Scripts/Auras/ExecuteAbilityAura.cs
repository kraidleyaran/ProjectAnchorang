using Assets.Scripts.Abilities;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Execute Ability Aura", menuName = @"Anchorang/Aura/Execute Ability", order = 0)]
    public class ExecuteAbilityAura : Aura
    {
        public Ability Ability;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            Ability.UseAbility(_controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}