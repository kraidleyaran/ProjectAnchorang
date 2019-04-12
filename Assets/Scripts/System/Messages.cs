using System;
using System.Collections.Generic;
using Assets.Scripts.Abilities;
using Assets.Scripts.Animation;
using Assets.Scripts.Auras;
using Assets.Scripts.System.Input;
using MessageBusLib;
using UnityEngine;

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

    public class MovementInfoMessage : EventMessage
    {
        public float Accleration { get; set; }
        public float MaxSpeed { get; set; }
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
        public float? MaxDistance { get; set; }
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

    public class AimDirectionMessage : EventMessage
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

    
    
}

