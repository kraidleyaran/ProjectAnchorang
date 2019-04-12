using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;

namespace Assets.Scripts.Auras
{
    //[CreateAssetMenu(fileName = "Apply To Anchorang Projectiles Aura", menuName = @"Anchorang/Aura/Apply to Anchorang Projectiles")]
    public class ApplyToAnchorangProjectileAura : Aura
    {
        public List<Aura> AnchorangProjectileAuras;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new ApplyAurasToCurrentAnchorangsMessage{Auras = AnchorangProjectileAuras}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}