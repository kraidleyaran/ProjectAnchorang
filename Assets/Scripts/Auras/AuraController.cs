using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{

    public class AuraController : MonoBehaviour
    {
        public Aura CurrentAura;

        public void Setup(Aura aura)
        {
            CurrentAura = Instantiate(aura, transform);
            CurrentAura.SubscribeController(this);
            name = $"{aura.Name} Aura Controller";
        }

        public void Destroy()
        {
            CurrentAura.Destroy();
            CurrentAura = null;
            gameObject.UnsubscribeFromAllMessages();
            Destroy(gameObject);
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
            if (CurrentAura)
            {
                CurrentAura.Destroy();
            }
            //CurrentAura.Destroy();
            gameObject.UnsubscribeFromAllMessages();
        }
    }
}