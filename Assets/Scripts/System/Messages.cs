using System;
using System.Collections.Generic;
using Assets.Scripts.Abilities;
using Assets.Scripts.Animation;
using Assets.Scripts.Auras;
using Assets.Scripts.System.Input;
using Assets.Scripts.UI;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.System
{
    public class InputStateMessage : EventMessage
    {
        public InputState PreviousState { get; set; }
        public InputState CurrentState { get; set; }
    }

    public class SetMovementDirectionMessage : EventMessage
    {
        public Vector2 Direction { get; set; }
    }

    public class SetFacingDirectionMessage : EventMessage
    {
        public Vector2 Direction { get; set; }
    }

    public class SetUnitAnimationStateMessage : EventMessage
    {
        public UnitAnimationState State { get; set; }
    }

    public class UpdateAnimationStateMessage : EventMessage
    {
        public UnitAnimationState State { get; set; }
    }

    public class SetupProjectileMessage : EventMessage
    {
        public ShootProjectileAura Aura { get; set; }
        public GameObject Owner { get; set; }
        public Vector2 Direction { get; set; }
    }

    public class SetStickSensitivityMessage : EventMessage
    {
        public RectTransform.Axis Axis { get; set; }
        public int Stick { get; set; }
        public float Sensitivity { get; set; }
    }

    public class UpdateStickSensitivityMessage : EventMessage
    {
        public RectTransform.Axis Axis { get; set; }
        public int Stick { get; set; }
        public float Sensitivity { get; set; }
    }

    public class SetPlayerAccelerationMessage : EventMessage
    {
        public float Acceleration { get; set; }
    }

    public class SetPlayerMaxSpeedMessage : EventMessage
    {
        public float MaxSpeed { get; set; }
    }

    public class SetAccelerationMessage : EventMessage
    {
        public float Acceleration { get; set; }
    }

    public class SetMaxSpeedMessage : EventMessage
    {
        public float MaxSpeed { get; set; }
    }

    public class RequestMovementInfoMessage : EventMessage
    {
        
    }

    public class UpdateMovementInfoMessage : EventMessage
    {
        public MovementInfo MovementInfo { get; set; }
    }


    public class UpdatePlayerInfoMessage : EventMessage
    {
        public float Acceleration { get; set; }
        public float MaxSpeed { get; set; }
    }

    public class MoveCameraToMessage : EventMessage
    {
        public Vector2 Position { get; set; }
    }

    public class SetAimDirectionMessage : EventMessage
    {
        public Vector2 Direction { get; set; }
    }

    public class ShootProjectileMessage : EventMessage
    {
        public ShootProjectileAura Aura { get; set; }
    }

    public class ObjectHitMessage : EventMessage
    {
        public GameObject ObjectHit { get; set; }
    }

    public class SetMaxDistanceMessage : EventMessage
    {
        public float MaxDistance { get; set; }
    }

    public class ResetMovementSpeedMessage : EventMessage
    {

    }

    public class SetTargetDestinationMessage : EventMessage
    {
        public GameObject Target { get; set; }
        public Action OnDestinationReached { get; set; }
    }

    public class DestinationReachedMessage : EventMessage
    {

    }

    public class SetUnitRuntimeAnimatorMessage : EventMessage
    {
        public RuntimeAnimatorController Animator { get; set; }
    }

    public class RegisterAnchorangMessage : EventMessage
    {
        public GameObject AnchorangProjectile { get; set; }
    }

    public class UnReigsterAnchorangMessage : EventMessage
    {
        public GameObject AnchorangProjectile { get; set; }
    }

    public class UseAbilityMessage : EventMessage
    {
        public Ability Ability { get; set; }
    }

    public class RegisterObjectForInputMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }

    public class UnregisterObjectForInputMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }

    public class AddAuraToObjectMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class AddAuraByControllerToObjectMessage : EventMessage
    {
        public AuraController Aura { get; set; }
    }

    public class RemoveAuraFromObjectMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class RemoveAuraByControllerFromObjectMessage : EventMessage
    {
        public AuraController Controller { get; set; }
    }

    public class RequestAimDirectionMessage : EventMessage
    {

    }

    public class SetOwnerMessage : EventMessage
    {
        public GameObject Owner { get; set; }
    }

    public class RequestOwnerMessage : EventMessage
    {
        
    }

    public class AddAuraCooldownMessage : EventMessage
    {
        public Aura Aura { get; set; }
        public float Cooldown { get; set; }
        public bool ApplyAfterCooldown { get; set; }
    }

    public class RemoveAuraCooldownMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class AuraCooldownCheckMessage : EventMessage
    {
        public Predicate<Aura> Predicate { get; set; }
        public Action<List<Aura>>  Action { get; set; }
    }

    public class UpdateAimDirectionMessage : EventMessage
    {
        public Vector2 Direction { get; set; }
    }

    public class SetAnchorangStateMessage : EventMessage
    {
        public AnchorangState State { get; set; }
    }

    public class RequestAnchorangStateMessage : EventMessage
    {

    }

    public class CallAnchorangMessage : EventMessage
    {
        
    }

    public class ApplyAurasToCurrentAnchorangsMessage : EventMessage
    {
        public List<Aura> Auras { get; set; }
    }

    public class AuraCheckMessage : EventMessage
    {
        public Predicate<Aura> Predicate { get; set; }
        public Action<List<Aura>> DoAfter { get; set; }
    }

    public class AdjustAccelerationMessage : EventMessage
    {
        public float Amount { get; set; }
        public AdjustmentType AdjustmentType { get; set; }
    }

    public class AdjustMaxSpeedMessage : EventMessage
    {
        public float Amount { get; set; }
        public AdjustmentType AdjustmentType { get; set; }
    }

    public class AdjustMovementValueMessage : EventMessage
    {
        public float Amount { get; set; }
        public MovementValueType MovementValueType { get; set; }
    }

    public class SetPositionMessage : EventMessage
    {
        public Vector2 Position { get; set; }
    }

    public class SetRelativePositionMessage : EventMessage
    {
        public Vector2 RelativePosition { get; set; }
    }

    public class RegisterTargetAimReticuleMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }

    public class ObjectLeftMessage : EventMessage
    {
        public GameObject Object { get; set; }
    }

    public class SetHoverTargetMessage : EventMessage
    {
        public GameObject Target { get; set; }
    }

    public class SetLockedTargetMessage : EventMessage
    {

    }

    public class SetOutlineMessage : EventMessage
    {
        public Color Color { get; set; }
        public int Thickness { get; set; }
    }

    public class ApplyFlashColorEffectMessage : EventMessage
    {
        public Color ChangeToColor { get; set; }
        public int Flashes { get; set; }
        public float LengthBetweenFlashes { get; set; }
    }

    public class MaxDistanceReachedMessage : EventMessage
    {

    }

    public class OnDemandEventMessage : EventMessage
    {
        public string EventName { get; set; }
    }

    public class LockAnimationStateMessage : EventMessage
    {
        public bool Locked { get; set; }
    }

    public class DistanceFromTargetMessage : EventMessage
    {
        public float Distance { get; set; }
    }

    public class AnimationCheckMessage : EventMessage
    {
        public List<UnitAnimationState> States { get; set; }
        public Action DoAfter { get; set; }
    }

    public class ChangeSpriteColorMessage : EventMessage
    {
        public Color FromColor { get; set; }
        public Color ToColor { get; set; }
    }

    public class SetTunicColorMessage : EventMessage
    {
        public Color Color { get; set; }
    }

    public class SetCurrentStanceMessage : EventMessage
    {
        public StanceAura Stance { get; set; }
    }

    public class AdvanceStanceMessage : EventMessage
    {

    }

    public class RequestStaminaMessage : EventMessage
    {
        
    }

    public class AdjustStaminaMessage : EventMessage
    {
        public int Stamina { get; set; }
    }


    public class UpdateStaminaMessage : EventMessage
    {
        public float CurrentStamina { get; set; }
        public int MaximumStamina { get; set; }
    }

    public class UpdatePlayerStaminaMessage : EventMessage
    {
        public float CurrentStamina { get; set; }
        public int MaximumStamina { get; set; }
    }

    public class SetStaminaRegenMessage : EventMessage
    {
        public int StaminaRegen { get; set; }
    }

    public class SetAnchorangThrowCostMessage : EventMessage
    {
        public int Stamina { get; set; }
    }

    public class ThrowAnchorangMessage : EventMessage
    {

    }

    public class RequestInputStatusMessage : EventMessage
    {

    }

    public class UpdateInputStatusMessage : EventMessage
    {
        public bool AllowInput { get; set; }
    }

    public class SetDashStateMessage : EventMessage
    {
        public bool IsDashing { get; set; }
    }

    public class SetDashStaminaCostMessage : EventMessage
    {
        public int StaminaCost { get; set; }
    }

    public class SetTweenedDestinationMessage : EventMessage
    {
        public Vector2 Destination { get; set; }
        public float Time { get; set; }
        public bool SpeedBased { get; set; }
        public Action OnComplete { get; set; }
        public Ease Ease { get; set; }
    }

    public class FinishedTweendDestinationMessage : EventMessage
    {
        public bool Complete { get; set; }
    }

    public class TakeDamageMeessage : EventMessage
    {
        public int Damage { get; set; }
    }

    public class InvincibilityStatusMessage : EventMessage
    {
        public bool IsInvincible { get; set; }
    }

    public class UpdateHealthMessage : EventMessage
    {
        public int CurrentHealth { get; set; }
        public int MaximumHealth { get; set; }
    }

    public class UpdatePlayerHealthMessage : EventMessage
    {
        public int CurrentHealth { get; set; }
        public int MaximumHealth { get; set; }
    }

    public class RequestHealthMessage : EventMessage
    {

    }

    public class UpdateDashCooldownIconMessage : EventMessage
    {
        public float Time { get; set; }
    }

    public class AddAuraIconMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class RemoveAuraIconMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class ShowAuraCooldownMessage : EventMessage
    {
        public Aura Parent { get; set; }
        public float Time { get; set; }
    }

    public class HideAuraCooldownMessage : EventMessage
    {
        public Aura Parent { get; set; }
    }

    public class ShowEnemyInfoMessage : EventMessage
    {

    }

    public class CloseEnemyInfoMessage : EventMessage
    {

    }

    public class UpdateEnemyHealthMessage : EventMessage
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
    }

    public class UpdateStanceMessage : EventMessage
    {
        public StanceAura Stance { get; set; }
    }

    public class UpdateRequiredStaminamStatusMessage : EventMessage
    {
        public bool HasEnough { get; set; }
    }

    public class AddPlayerAuraIconMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class RemovePlayerAuraIconMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class ShowPlayerAuraCooldownMessage : EventMessage
    {
        public Aura Aura { get; set; }
        public float Time { get; set; }
    }

    public class HidePlayerAuraCooldownMessage : EventMessage
    {
        public Aura Aura { get; set; }
        public float Time { get; set; }
    }

    public class AddEnemyAuraIconMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class RemoveEnemyAuraIconMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class ShowEnemyCooldownAuraMessage : EventMessage
    {
        public Aura Aura { get; set; }
        public float Time { get; set; }
    }

    public class HideEnemyCooldownAuraMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class RequestUiAuraIconsMessage : EventMessage
    {

    }

    public class ShowRadialTimerForAuraMessage : EventMessage
    {
        public Aura Aura { get; set; }
        public float Time { get; set; }
        public float Start { get; set; }
        public float End { get; set; }
        public Image.FillMethod FillMethod { get; set; }
        public FillOrigin FillOrigin { get; set; }
        public bool Clockwise { get; set; }
    }

    public class HideRaidalTimerForAuraMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class ShowRadialTimerForPlayerAuraMessage : EventMessage
    {
        public Aura Aura { get; set; }
        public float Time { get; set; }
        public float Start { get; set; }
        public float End { get; set; }
        public Image.FillMethod FillMethod { get; set; }
        public FillOrigin FillOrigin { get; set; }
        public bool Clockwise { get; set; }
    }

    public class HideRaidlTimerForPlayerMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class ShowRaidalTimerForEnemyAuraMessage : EventMessage
    {
        public Aura Aura { get; set; }
        public float Time { get; set; }
        public float Start { get; set; }
        public float End { get; set; }
        public Image.FillMethod FillMethod { get; set; }
        public FillOrigin FillOrigin { get; set; }
        public bool Clockwise { get; set; }
    }

    public class HideRadialTimerForEnemyAuraMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class SetPlayerHoverTargetMessage : EventMessage
    {
        public GameObject Target { get; set; }
    }

    public class ShowAuraDescriptionMessage : EventMessage
    {
        public Aura Aura { get; set; }
    }

    public class ClearAuraDescriptionMessage : EventMessage
    {

    }

    public class SetPathingMessage : EventMessage
    {
        public List<Vector2> Positions { get; set; }
    }

    public class SetAiStateMessage : EventMessage
    {
        public AiState State { get; set; }
    }

    public class UpdateAiStateMessage : EventMessage
    {
        public AiState State { get; set; }
    }

    public class RequestAiStateMessage : EventMessage
    {
        
    }

    public class RequestPlayerObjectMessage : EventMessage
    {

    }

    public class UpdateplayerObjectMessage : EventMessage
    {
        public GameObject Player { get; set; }
    }

    public class InvincibilityCheckMessage : EventMessage
    {
        public Action DoAfter { get; set; }
    }
}

