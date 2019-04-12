using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Color Change Effect Aura", menuName = @"Anchorang/Aura/Color Change Effect")]
    public class ColorChangeEffectAura : Aura
    {
        public Color ChangeTo;
        public int Flashes;
        public float LengthBetweenFlashes;

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.gameObject.SendMessageTo(new ApplyFlashColorEffectMessage{ChangeToColor = ChangeTo, Flashes = Flashes, LengthBetweenFlashes = LengthBetweenFlashes}, _controller.transform.parent.gameObject);
            _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
        }


    }
}