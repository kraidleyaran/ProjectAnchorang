using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Sound Aura", menuName = @"Anchorang/Aura/Sound")]
    public class SoundAura : Aura
    {
        public AudioClip Clip;

        private SoundController _soundController { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _soundController = Instantiate(FactoryController.SOUND, _controller.transform.parent);
            _soundController.Setup(Clip);
        }

        public override void Update()
        {
            if (!_soundController.Source.isPlaying)
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            Destroy(_soundController.gameObject);
        }
    }
}