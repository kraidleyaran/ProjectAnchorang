using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Animation Aura", menuName = @"Anchorang/Aura/Animation", order = 0)]
    public class AnimationAura : Aura
    {
        [Header("Animation Aura Settings")]
        public RuntimeAnimatorController RuntimeController;
        public int SortingOrder = 0;
        public Color DefaultColor = Color.white;
        public Vector2 Scale = Vector2.one;

        private UnitAnimationController _animationController { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _animationController = Instantiate(FactoryController.UNIT_ANIMATION_CONTROLLER, _controller.transform.parent);
            _animationController.SpriteRenderer.color = DefaultColor;
            _animationController.SpriteRenderer.sortingOrder = SortingOrder;
            _animationController.SpriteRenderer.transform.localScale = Scale;
            _controller.gameObject.SendMessageTo(new SetUnitRuntimeAnimatorMessage{Animator = RuntimeController}, _controller.transform.parent.gameObject);
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_animationController.gameObject)
            {
                Destroy(_animationController.gameObject);
            }
        }
    }
}