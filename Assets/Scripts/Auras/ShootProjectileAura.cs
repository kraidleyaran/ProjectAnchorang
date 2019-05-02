using System.Collections.Generic;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Shoot Projectile Aura", menuName = @"Anchorang/Aura/Shoot Projectile", order = 0)]
    public class ShootProjectileAura : Aura
    {
        [Header("Shoot Projectile Aura Settings")]
        public List<Aura> ProjectileAuras;
        public int MaxProjectiles = 1;
        public float DistanceInFrontOfPlayer = .5f;
        public float Angle = 0f;

        private List<UnitController> _projectiles { get; set; }
        private Sequence _timeBetweenProjectiles { get; set; }
        private Vector2 _aimDireciton { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _projectiles = new List<UnitController>();
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateAimDirectionMessage>(AimDirection, _instanceId);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestAimDirectionMessage(), _controller.transform.parent.gameObject);
            //_controller.gameObject.SendMessageTo(new ShootProjectileMessage{Aura = this}, _controller.transform.parent.gameObject);
        }

        public override void Update()
        {
            if (_projectiles.Count < MaxProjectiles)
            {
                if (_timeBetweenProjectiles == null)
                {
                    var position = _controller.transform.position.ToVector2();
                    position += Vector2.ClampMagnitude(_aimDireciton * DistanceInFrontOfPlayer, DistanceInFrontOfPlayer);
                    var projectile = Instantiate(FactoryController.UNIT, position, Quaternion.identity);
                    foreach (var aura in ProjectileAuras)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage { Aura = aura }, projectile.gameObject);
                    }
                    var aimDirection = Quaternion.Euler(0, 0, Angle) * _aimDireciton;
                    _controller.gameObject.SendMessageTo(new SetMovementDirectionMessage { Direction = aimDirection  }, projectile.gameObject);
                    _controller.gameObject.SendMessageTo(new SetOwnerMessage{Owner = _controller.transform.parent.gameObject}, projectile.gameObject);
                    _projectiles.Add(projectile);
                }
            }
            else
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            }
        }

        private void AimDirection(UpdateAimDirectionMessage msg)
        {
            _aimDireciton = msg.Direction;
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateAimDirectionMessage>(_instanceId);
        }
    }
}