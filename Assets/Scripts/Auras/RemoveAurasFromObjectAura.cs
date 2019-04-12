using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Remove Auras from Object Aura", menuName = @"Anchorang/Aura/Remove Auras from Object")]
    public class RemoveAurasFromObjectAura : Aura
    {
        [Header("Remove Aura From Object Aura Settings")]
        public List<Aura> AurasToRemove;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            foreach (var aura in AurasToRemove)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraFromObjectMessage{Aura = aura}, _controller.transform.parent.gameObject);
            }
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}