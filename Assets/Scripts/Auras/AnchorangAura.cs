using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Anchorang Aura", menuName = @"Anchorang/Aura/Anchorang")]
    public class AnchorangAura : Aura
    {

        [Header("Anchorang Aura Settings")]
        public int MaxAnchorangs = 1;
        public AbilityInputAura ThrowAnchorang;

        private AnchorangState _state { get; set; }
        private List<GameObject> _projectiles { get; set; }


        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _projectiles = new List<GameObject>();
            _controller.transform.parent.gameObject.SubscribeWithFilter<RegisterAnchorangMessage>(RegisterAnchorang, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UnReigsterAnchorangMessage>(UnRegisterAnchorang, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetAnchorangStateMessage>(SetAnchorangState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RequestAnchorangStateMessage>(RequestAnchorangState, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<ApplyAurasToCurrentAnchorangsMessage>(ApplyAurasToCurrentAnchorangs, _instanceId);
        }

        private void RegisterAnchorang(RegisterAnchorangMessage msg)
        {
            _projectiles.Add(msg.AnchorangProjectile);
        }

        private void UnRegisterAnchorang(UnReigsterAnchorangMessage msg)
        {
            _projectiles.Remove(msg.AnchorangProjectile);
        }

        private void SetAnchorangState(SetAnchorangStateMessage msg)
        {
            
            switch (_state)
            {
                case AnchorangState.Caught:
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = ThrowAnchorang}, _controller.transform.parent.gameObject);
                    break;
                case AnchorangState.Thrown:
                    _controller.gameObject.SendMessageTo(new RemoveAuraFromObjectMessage{Aura = ThrowAnchorang }, _controller.transform.parent.gameObject);
                    break;
            }
            
            
        }

        private void RequestAnchorangState(RequestAnchorangStateMessage msg)
        {
            _controller.gameObject.SendMessageTo(new SetAnchorangStateMessage{State = _state}, _controller.transform.parent.gameObject);
        }

        private void ApplyAurasToCurrentAnchorangs(ApplyAurasToCurrentAnchorangsMessage msg)
        {
            /*
            foreach (var projectile in _projectiles)
            {
                foreach (var aura in msg.Auras)
                {
                    _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, projectile);
                }
            }
            */
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RegisterAnchorangMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UnReigsterAnchorangMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetAnchorangStateMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestAnchorangStateMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ApplyAurasToCurrentAnchorangsMessage>(_instanceId);
        }
    }
}