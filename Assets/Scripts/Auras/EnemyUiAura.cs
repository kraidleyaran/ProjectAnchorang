using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Enemy Ui Aura", menuName = @"Anchorang/Aura/Ui/Enemy Ui", order = 0)]
    public class EnemyUiAura : Aura
    {
        private static int _enemyUiCount { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _enemyUiCount++;
            if (_enemyUiCount <= 1)
            {
                _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateHealthMessage>(UpdateHealth, _instanceId);
                _controller.transform.parent.gameObject.SubscribeWithFilter<AddAuraIconMessage>(AddAuraIcon, _instanceId);
                _controller.transform.parent.gameObject.SubscribeWithFilter<RemoveAuraIconMessage>(RemoveAuraIcon, _instanceId);
                _controller.transform.parent.gameObject.SubscribeWithFilter<ShowAuraCooldownMessage>(ShowAuraCooldown, _instanceId);
                _controller.transform.parent.gameObject.SubscribeWithFilter<HideAuraCooldownMessage>(HideAuraCooldown, _instanceId);
                _controller.transform.parent.gameObject.SendMessageTo(new RequestHealthMessage(), _controller.transform.parent.gameObject);
                _controller.gameObject.SendMessage(new ShowEnemyInfoMessage());
                _controller.transform.parent.gameObject.SendMessageTo(new RequestUiAuraIconsMessage(), _controller.transform.parent.gameObject);
            }
            else
            {
                _controller.gameObject.SendMessageTo(new RemoveAuraByControllerFromObjectMessage{Controller = _controller}, _controller.transform.parent.gameObject);
            }

        }

        private void UpdateHealth(UpdateHealthMessage msg)
        {
            _controller.gameObject.SendMessage(new UpdateEnemyHealthMessage{CurrentHealth = msg.CurrentHealth, MaxHealth = msg.MaximumHealth});
        }

        private void AddAuraIcon(AddAuraIconMessage msg)
        {
            _controller.gameObject.SendMessage(new AddEnemyAuraIconMessage{Aura = msg.Aura});
        }

        private void RemoveAuraIcon(RemoveAuraIconMessage msg)
        {
            _controller.gameObject.SendMessage(new RemoveEnemyAuraIconMessage{Aura = msg.Aura});
        }

        private void ShowAuraCooldown(ShowAuraCooldownMessage msg)
        {
            _controller.gameObject.SendMessage(new ShowEnemyCooldownAuraMessage{Aura = msg.Parent, Time = msg.Time});
        }

        private void HideAuraCooldown(HideAuraCooldownMessage msg)
        {
            _controller.gameObject.SendMessage(new HideEnemyCooldownAuraMessage{Aura = msg.Parent});
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateHealthMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AddAuraIconMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RemoveAuraIconMessage>(_instanceId);
            _enemyUiCount--;
            if (_enemyUiCount <= 0)
            {
                _controller.gameObject.SendMessage(new CloseEnemyInfoMessage());
            }
        }
    }
}