using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Player Ui Aura", menuName = @"Anchorang/Aura/Ui/Player Ui", order = 0)]
    public class PlayerUiAura : Aura
    {
        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateStaminaMessage>(UpdateStamina, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<UpdateHealthMessage>(UpdateHealth, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<AddAuraIconMessage>(AddAuraIcon, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<RemoveAuraIconMessage>(RemoveAuraIcon, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<ShowAuraCooldownMessage>(ShowAuraCooldown, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<HideAuraCooldownMessage>(HideAuraCooldown, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<ShowRadialTimerForAuraMessage>(ShowRaidalTimerForAura, _instanceId);
            _controller.transform.parent.gameObject.SubscribeWithFilter<HideRaidalTimerForAuraMessage>(HideRadialTimerForAura, _instanceId);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestStaminaMessage(), _controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestHealthMessage(), _controller.transform.parent.gameObject);
            _controller.transform.parent.gameObject.SendMessageTo(new RequestUiAuraIconsMessage(), _controller.transform.parent.gameObject);
        }

        private void UpdateStamina(UpdateStaminaMessage msg)
        {
            _controller.gameObject.SendMessage(new UpdatePlayerStaminaMessage{CurrentStamina = msg.CurrentStamina, MaximumStamina = msg.MaximumStamina});
        }

        private void UpdateHealth(UpdateHealthMessage msg)
        {
            _controller.gameObject.SendMessage(new UpdatePlayerHealthMessage{CurrentHealth = msg.CurrentHealth, MaximumHealth = msg.MaximumHealth});
        }

        private void AddAuraIcon(AddAuraIconMessage msg)
        {
            _controller.gameObject.SendMessage(new AddPlayerAuraIconMessage{Aura = msg.Aura});
        }

        private void RemoveAuraIcon(RemoveAuraIconMessage msg)
        {
            _controller.gameObject.SendMessage(new RemovePlayerAuraIconMessage{Aura = msg.Aura});
        }

        private void ShowAuraCooldown(ShowAuraCooldownMessage msg)
        {
            _controller.gameObject.SendMessage(new ShowPlayerAuraCooldownMessage{Aura = msg.Parent, Time = msg.Time});
        }

        private void HideAuraCooldown(HideAuraCooldownMessage msg)
        {
            _controller.gameObject.SendMessage(new HideAuraCooldownMessage{Parent = msg.Parent});
        }

        private void ShowRaidalTimerForAura(ShowRadialTimerForAuraMessage msg)
        {
            _controller.gameObject.SendMessage(new ShowRadialTimerForPlayerAuraMessage
            {
                Aura = msg.Aura,
                Time = msg.Time,
                Start = msg.Start,
                End = msg.End,
                Clockwise = msg.Clockwise,
                FillOrigin = msg.FillOrigin,
                FillMethod = msg.FillMethod
            });
        }

        private void HideRadialTimerForAura(HideRaidalTimerForAuraMessage msg)
        {
            _controller.gameObject.SendMessage(new HideRaidlTimerForPlayerMessage{Aura = msg.Aura});
        }

        public override void Destroy()
        {
            base.Destroy();
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateStaminaMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<UpdateHealthMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<AddAuraIconMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<RemoveAuraIconMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<ShowAuraCooldownMessage>(_instanceId);
            _controller.transform.parent.gameObject.UnsubscribeFromFilter<HideAuraCooldownMessage>(_instanceId);

        }
    }
}