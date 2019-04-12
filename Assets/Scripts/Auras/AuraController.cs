using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{

    public class AuraController : MonoBehaviour
    {
        public Aura CurrentAura;

        public void Setup(Aura aura)
        {
            CurrentAura = Instantiate(aura);
            CurrentAura.SubscribeController(this);
            name = $"{aura.Name} Aura Controller";
        }

        void Update()
        {
            CurrentAura.Update();
        }

        void FixedUpdate()
        {
            CurrentAura.FixedUpdate();
        }

        void OnDestroy()
        {
            CurrentAura.Destroy();
            gameObject.UnsubscribeFromAllMessages();
        }
    }
}