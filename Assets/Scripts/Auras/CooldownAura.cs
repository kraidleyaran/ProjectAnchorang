using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Cooldown Aura", menuName = @"Anchorang/Aura/Cooldown")]
    public class CooldownAura : Aura
    {
        private List<AuraCooldown> _cooldowns { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _cooldowns = new List<AuraCooldown>();
            _controller.transform.parent.gameObject.SubscribeWithFilter<AddAuraCooldownMessage>(AddAuraCooldown, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RemoveAuraCooldownMessage>(RemoveAuraCooldown, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AuraCooldownCheckMessage>(AuraCooldownCheck, _instanceId);
        }

        private void AddAuraCooldown(AddAuraCooldownMessage msg)
        {
            var currentCooldown = _cooldowns.Find(c => c.Aura.Name == msg.Aura.Name);
            if (currentCooldown != null)
            {
                if (currentCooldown.Cooldown != null && currentCooldown.Cooldown.IsActive())
                {
                    currentCooldown.Cooldown.Kill();
                }
                _cooldowns.Remove(currentCooldown);
            }
            else
            {
                currentCooldown = new AuraCooldown{Aura = msg.Aura};
            }
            currentCooldown.ApplyAfterCooldown = msg.ApplyAfterCooldown;
            if (msg.Cooldown > 0)
            {
                currentCooldown.Cooldown = DOTween.Sequence().AppendInterval(msg.Cooldown).SetEase(Ease.Linear)
                    .OnComplete(
                        () =>
                        {
                            currentCooldown.Cooldown = null;
                            _controller.gameObject.SendMessageTo(new RemoveAuraCooldownMessage{Aura = msg.Aura}, _controller.transform.parent.gameObject);
                        });
            }
            _cooldowns.Add(currentCooldown);
        }

        private void RemoveAuraCooldown(RemoveAuraCooldownMessage msg)
        {
            var cooldown = _cooldowns.Find(c => c.Aura.Name == msg.Aura.Name);
            if (cooldown != null)
            {
                if (cooldown.Cooldown != null && cooldown.Cooldown.IsActive())
                {
                    cooldown.Cooldown.Kill();
                    cooldown.Cooldown = null;
                }
                _cooldowns.Remove(cooldown);
                if (cooldown.ApplyAfterCooldown)
                {
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = cooldown.Aura}, _controller.transform.parent.gameObject);
                }
            }
        }

        private void AuraCooldownCheck(AuraCooldownCheckMessage msg)
        {
            var auraCooldowns = _cooldowns.FindAll(a => msg.Predicate.Invoke(a.Aura)).Select(a => a.Aura).ToList();
            msg.Action?.Invoke(auraCooldowns);
        }

        public override void Destroy()
        {
            base.Destroy();
            foreach (var cooldown in _cooldowns)
            {
                if (cooldown.Cooldown != null && cooldown.Cooldown.IsActive())
                {
                    cooldown.Cooldown.Kill();
                    cooldown.Cooldown = null;
                }
            }
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AddAuraCooldownMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RemoveAuraCooldownMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AuraCooldownCheckMessage>(_instanceId);
            
        }
    }
}