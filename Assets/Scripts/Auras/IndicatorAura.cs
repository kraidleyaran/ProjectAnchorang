using Assets.Scripts.System;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Indicator Aura", menuName = @"Anchorang/Aura/Indicator")]
    public class IndicatorAura : Aura
    {
        public GameObject Indicator;
        public Vector2 Position;

        private GameObject _indicator { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _indicator = Instantiate(Indicator, _controller.transform);
            _indicator.transform.localPosition = Vector2.zero + Position;
        }
    }
}