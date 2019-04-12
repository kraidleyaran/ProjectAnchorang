using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Outline Aura", menuName = @"Anchorang/Aura/Outline")]
    public class OutlineAura : Aura
    {
        public Color Color;
        public int Thickness;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new SetOutlineMessage{Color = Color, Thickness = Thickness}, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.gameObject.SendMessageTo(new SetOutlineMessage{Color = Color.white, Thickness = 0}, _controller.transform.parent.gameObject);
        }
    }
}