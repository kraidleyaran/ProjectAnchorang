using Assets.Scripts.Auras;
using UnityEngine;

namespace Assets.Scripts.System
{
    public class FactoryController : MonoBehaviour
    {
        public static AuraController AURA { get; private set; }
        public static UnitController UNIT { get; private set; }
        public static UnitAnimationController UNIT_ANIMATION_CONTROLLER { get; private set; }
        public static SoundController SOUND { get; private set; }

        private static FactoryController _instance { get; set; }

        [Header("Template References")]
        public AuraController AuraTemplate;
        public UnitController UnitTemplate;
        public UnitAnimationController UnitAnimationTemplate;
        public SoundController SoundTemplate;

        void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            SetupStatics();
            _instance = this;

        }

        private void SetupStatics()
        {
            AURA = AuraTemplate;
            UNIT = UnitTemplate;
            UNIT_ANIMATION_CONTROLLER = UnitAnimationTemplate;
            SOUND = SoundTemplate;
        }
    }
}