using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Aura Check Aura", menuName = @"Anchorang/Aura/Aura Check", order = 0)]
    public class AuraCheckAura : Aura
    {
        public List<Aura> RequiredAuras;
        public List<Aura> AurasToApply;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new AuraCheckMessage{Predicate = a => RequiredAuras.Exists(t => t.Name == a.Name), DoAfter =
                auras =>
                {
                    foreach (var aura in AurasToApply)
                    {
                        _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, _controller.transform.parent.gameObject );
                    }
                }}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}