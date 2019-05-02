using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Tunic Aura", menuName = @"Anchorang/Aura/Tunic")]
    public class TunicAura : Aura
    {
        public Color StartingColor;

        private Color _currentColor { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _currentColor = StartingColor;
            _controller.transform.parent.gameObject.SubscribeWithFilter<SetTunicColorMessage>(SetTunicColor, _instanceId);
        }

        private void SetTunicColor(SetTunicColorMessage msg)
        {
            _controller.gameObject.SendMessageTo(new ChangeSpriteColorMessage{FromColor = _currentColor, ToColor = msg.Color}, _controller.transform.parent.gameObject);
            _currentColor = msg.Color;
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<SetTunicColorMessage>(_instanceId);
        }
    }
}