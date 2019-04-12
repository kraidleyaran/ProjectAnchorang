using System.Collections.Generic;
using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Create Object Aura", menuName = @"Anchorang/Aura/Create Object", order = 0)]
    public class CreateObjectAura : Aura
    {
        public List<Aura> ObjectAuras;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            var newObject = Instantiate(FactoryController.UNIT, _controller.transform.position, Quaternion.identity);
            foreach (var aura in ObjectAuras)
            {
                _controller.gameObject.SendMessageTo(new AddAuraToObjectMessage{Aura = aura}, newObject.gameObject);
            }
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }
    }
}