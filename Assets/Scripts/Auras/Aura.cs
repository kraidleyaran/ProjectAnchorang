using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Generic Aura", menuName = @"Anchorang/Aura/Generic", order = 0)]
    public class Aura : ScriptableObject
    {
        [Header("General Aura Settings", order = 0)]
        public string Name;
        [TextArea]
        public string Description;
        public int MaxStack = 1;
        public AuraType AuraType;

        [Header("General Ui Settings")]
        public Sprite Sprite;
        public bool ShowInUi;

        [Header("General Debug Settings")]
        public bool DebugAura = false;

        protected internal AuraController _controller { get; set; }
        protected internal string _instanceId { get; private set; }
        
        public virtual string AuraDescription { get; }


        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void SubscribeController(AuraController controller)
        {
            _controller = controller;
            _instanceId = _controller.GetInstanceID().ToString();
            if (ShowInUi)
            {
                _controller.gameObject.SendMessageTo(new AddAuraIconMessage{Aura = this}, _controller.transform.parent.gameObject);
                _controller.transform.parent.gameObject.SubscribeWithFilter<RequestUiAuraIconsMessage>(RequestUiAuraIcons, _instanceId);
            }
        }

        public virtual void Destroy()
        {
            _controller.gameObject.UnsubscribeFromAllMessages();
            if (ShowInUi)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraIconMessage{Aura = this}, _controller.transform.parent.gameObject);
                _controller.transform.parent.gameObject.UnsubscribeFromFilter<RequestUiAuraIconsMessage>(_instanceId);
            }
        }

        private void RequestUiAuraIcons(RequestUiAuraIconsMessage msg)
        {
            _controller.gameObject.SendMessageTo(new AddAuraIconMessage{Aura = this}, msg.Sender);
        }
    }
}