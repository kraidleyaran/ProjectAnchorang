using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Health Aura", menuName = @"Anchorang/Aura/Stats/Health", order = 0)]
    public class HealthAura : Aura
    {        
        [Header("Health Aura Settings")]
        public int StartingHealth = 5;
        public int StartingMaximumhealth = 5;
        public List<Aura> ApplyOnDamageTaken;
        public Aura TakeDamageColorEffect;
        public List<Aura> ApplyOnDeath;

        private int _health { get; set; }
        private int _maximumHealth { get; set; }
        private bool _isInvincible { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _health = StartingHealth;
            _maximumHealth = StartingMaximumhealth;
            _controller.transform.parent.gameObject.SubscribeWithFilter<TakeDamageMeessage>(TakeDamage, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<InvincibilityStatusMessage>(InvincibilityStatus, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestHealthMessage>(RequestHealth, _instanceId);
            _controller.gameObject.SendMessageTo(new UpdateHealthMessage{CurrentHealth = _health, MaximumHealth = _maximumHealth}, _instanceId);
        }

        private void TakeDamage(TakeDamageMeessage msg)
        {
            if (!_isInvincible)
            {
                _health -= msg.Damage;
                if (_health <= 0)
                {
                    _health = 0;
                    foreach (var aura in ApplyOnDeath)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
                    }
                }
                else
                {
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = TakeDamageColorEffect }, _controller.transform.parent.gameObject);
                    foreach (var aura in ApplyOnDamageTaken)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
                    }
                }
                _controller.gameObject.SendMessageTo(new UpdateHealthMessage{CurrentHealth = _health, MaximumHealth = _maximumHealth}, _controller.transform.parent.gameObject);
                
            }
        }

        private void InvincibilityStatus(InvincibilityStatusMessage msg)
        {
            _isInvincible = msg.IsInvincible;
        }

        private void RequestHealth(RequestHealthMessage msg)
        {
            _controller.gameObject.SendMessageTo(new UpdateHealthMessage{CurrentHealth = _health, MaximumHealth = _maximumHealth}, msg.Sender);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<TakeDamageMeessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<InvincibilityStatusMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestHealthMessage>(_instanceId);
        }
    }
}