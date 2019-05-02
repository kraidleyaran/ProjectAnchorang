using System.Collections.Generic;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Knockback Effect Aura", menuName = @"Anchorang/Aura/Effect/Knockback", order = 0)]
    public class KnobackEffectAura : Aura
    {
        [Header("Knockback Settings")]
        public float KnockbackDistance;
        public float KnockbackSpeed;
        public List<Aura> AffectedAuras;
        public HitboxController KnockbackHitbox;

        private List<GameObject> _affectedObjects = new List<GameObject>();
        private HitboxController _hitBox { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SubscribeWithFilter<ObjectHitMessage>(ObjectHit, _instanceId);
            _controller.gameObject.SubscribeWithFilter<ObjectLeftMessage>(ObjectLeft, _instanceId);
            _hitBox = Instantiate(KnockbackHitbox, _controller.transform.parent);
            _hitBox.Setup(_controller);
        }

        private void ObjectHit(ObjectHitMessage msg)
        {
            if (!_affectedObjects.Contains(msg.ObjectHit))
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
                    }, msg.ObjectHit);
                }
                else
                {
                    apply = true;
                }
                _controller.gameObject.SendMessageTo(new InvincibilityCheckMessage
                {
                    DoAfter = () =>
                    {
                        apply = false;
                    }
                }, msg.ObjectHit);
                if (apply)
                {
                    var objectPosition = msg.ObjectHit.transform.position;
                    var direction = msg.ObjectHit.transform.position - _controller.transform.parent.position;
                    objectPosition += direction * KnockbackDistance;
                    _controller.gameObject.SendMessageTo(new SetTweenedDestinationMessage { Destination = objectPosition, Ease = Ease.Linear, Time = KnockbackSpeed, SpeedBased = true}, msg.ObjectHit);
                }
            }
        }

        private void ObjectLeft(ObjectLeftMessage msg)
        {
            _affectedObjects.Remove(msg.Object);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ObjectHitMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ObjectLeftMessage>(_instanceId);
        }
    }
}