using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Ai Ability Manager", menuName = @"Anchorang/Aura/Ai/Ai Ability Manager")]
    public class AiAbilityManagerAura : Aura
    {
        public List<AiAbilityAura> Abilities;
        public float GlobalCooldown = 1f;
        public float DelayBeforeDoingAction = 3f;

        private Sequence _globalCooldown { get; set; }


        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _globalCooldown = DOTween.Sequence().AppendInterval(DelayBeforeDoingAction).OnComplete(() =>
            {
                _globalCooldown = null;
            });
        }

        public override void Update()
        {
            if (_globalCooldown == null && Abilities.Count > 0)
            {
                var abilities = Abilities.ToList();
                _controller.gameObject.SendMessageTo(new AuraCooldownCheckMessage{Predicate = aura => abilities.Exists(a => a.Name == aura.Name), Action =
                    auras =>
                    {
                        abilities.RemoveAll(a => auras.Exists(aura => aura.Name == a.Name));
                    }}, _controller.transform.parent.gameObject);
                if (abilities.Count > 0)
                {
                    var ability = abilities[0];
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = ability}, _controller.transform.parent.gameObject);
                    _globalCooldown = DOTween.Sequence().AppendInterval(GlobalCooldown).OnComplete(() =>
                    {
                        _globalCooldown = null;
                    });
                }
            }
        }
    }
}