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

        public bool DebugAura = false;

        protected internal AuraController _controller { get; set; }
        protected internal string _instanceId { get; private set; }


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
        }

        public virtual void Destroy()
        {
            _controller.gameObject.UnsubscribeFromAllMessages();
        }
           
    }
}