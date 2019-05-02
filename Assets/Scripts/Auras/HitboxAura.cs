using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Htibox Aura", menuName = @"Anchorang/Aura/Hitbox")]
    public class HitboxAura : Aura
    {
        [Header("Hitbox Aura Settings")]
        public HitboxController Hitbox;
        public List<Aura> AffectedAuras;

        public List<Aura> OnHitTargetAuras;
        public List<Aura> OnHitSelfAuras;

        public List<Aura> OnLeaveTargetAuras;
        public List<Aura> OnLeaveSelfAuras;

        public bool RemoveOnHit;
        
        private HitboxController _hitbox { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _hitbox = Instantiate(Hitbox, _controller.transform.parent);
            _hitbox.Setup(_controller);
            _controller.gameObject.SubscribeWithFilter<ObjectHitMessage>(ObjectHit, _instanceId);
            _controller.gameObject.SubscribeWithFilter<ObjectLeftMessage>(ObjectLeft, _instanceId);
        }

        private void ObjectHit(ObjectHitMessage msg)
        {
            if (msg.ObjectHit)
            {
                var apply = false;
                if (AffectedAuras.Count > 0)
                {
                    _controller.gameObject.SendMessageTo(new AuraCheckMessage{Predicate = aura => AffectedAuras.Exists(a => a.Name == aura.Name), DoAfter =
                        auras =>
                        {
                            apply = auras.Count > 0;
                        }}, msg.ObjectHit);
                }
                else
                {
                    apply = true;
                }
                _controller.gameObject.SendMessageTo(new InvincibilityCheckMessage{DoAfter = () =>
                {
                    //Object is invincible, do not apply
                    apply = false;
                }}, msg.ObjectHit);
                if (apply)
                {
                    
                    foreach (var aura in OnHitTargetAuras)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, msg.ObjectHit);
                    }
                    foreach (var aura in OnHitSelfAuras)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
                    }
                    //TODO: Remove this using an aura
                    if (RemoveOnHit)
                    {
                        _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage { Controller = _controller }, _controller.transform.parent.gameObject);
                    }
                }

            }
        }

        private void ObjectLeft(ObjectLeftMessage msg)
        {
            if (msg.Object)
            {
                var apply = false;
                if (AffectedAuras.Count > 0)
                {
                    _controller.gameObject.SendMessageTo(new AuraCheckMessage
                    {
                        Predicate = aura => AffectedAuras.Exists(a => a.Name == aura.Name),
                        DoAfter =
                            auras =>
                            {
                                apply = auras.Count > 0;
                            }
                    }, msg.Object);
                }
                else
                {
                    apply = true;
                }
                if (apply)
                {
                    foreach (var aura in OnLeaveTargetAuras)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, msg.Object);
                    }
                    foreach (var aura in OnLeaveSelfAuras)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, _controller.transform.parent.gameObject);
                    }
                }
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_hitbox)
            {
                Destroy(_hitbox.gameObject);
            }
            _controller.gameObject.UnsubscribeFromFilter<ObjectHitMessage>(_instanceId);
            _controller.gameObject.UnsubscribeFromFilter<ObjectLeftMessage>(_instanceId);
        }
    }
}