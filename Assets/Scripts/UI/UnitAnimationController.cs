using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Animation;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    [Header("Settings")]
    public bool FlipSpriteX;
    public List<Material> SpriteMats;
    public Material ColorChangeMaterial;

    public SpriteRenderer SpriteRenderer { get; set; }

    private Animator _animator { get; set; }
    private UnitAnimationState _currentAnimationState { get; set; }
    private SpriteOutline _outline { get; set; }
    private Tween _flashTween { get; set; }
    private bool _locked { get; set; }

    void Awake()
    {
        SpriteRenderer = transform.GetComponent<SpriteRenderer>();
        _animator = transform.GetComponent<Animator>();
        _outline = transform.GetComponent<SpriteOutline>();
        if (_animator && SpriteRenderer)
        {
            SubscribeToMessages();
        }
        //We have to do this because using a Mesh Renderer makes the animations not work, but a sprite renderer only holds a single Material in the inspector
        //So we instantiate the mats ourselves and attach them as needed
        var spriteMats = new List<Material>();
        foreach (var mat in SpriteMats)

        {
            var material = Instantiate(mat);
            material.name = mat.name;
            spriteMats.Add(mat);
        }
        SpriteRenderer.materials = spriteMats.ToArray();
    }

    public void SetAnimationState(float animationState)
    {
        gameObject.SendMessageTo(new SetUnitAnimationStateMessage{State = (UnitAnimationState)animationState}, transform.parent.gameObject);
    }

    public void OnDemandEvent(string eventName)
    {
        gameObject.SendMessageTo(new OnDemandEventMessage{EventName = eventName}, transform.parent.gameObject);
    }

    public void Unlock()
    {
        _locked = false;
    }

    private void SubscribeToMessages()
    {
        transform.parent.gameObject.Subscribe<SetUnitAnimationStateMessage>(SetUnitAnimationState);
        transform.parent.gameObject.Subscribe<SetUnitRuntimeAnimatorMessage>(SetUnitRuntimeAnimator);
        transform.parent.gameObject.Subscribe<UpdateAimDirectionMessage>(AimDirection);
        transform.parent.gameObject.Subscribe<SetOutlineMessage>(SetOutline);
        transform.parent.gameObject.Subscribe<ApplyFlashColorEffectMessage>(ApplyFlashColorEffect);
        transform.parent.gameObject.Subscribe<LockAnimationStateMessage>(LockAnimationState);
        transform.parent.gameObject.Subscribe<AnimationCheckMessage>(AnimationCheck);
        transform.parent.gameObject.Subscribe<ChangeSpriteColorMessage>(ChangeSpriteColor);
    }

    private Tween ChangeToColor(Color color, float time)
    {
        return SpriteRenderer.DOColor(color, time).SetEase(Ease.Linear);
    }

    private void AimDirection(UpdateAimDirectionMessage msg)
    {
        if (msg.Direction != Vector2.zero && !_locked)
        {
            var naturalDirection = msg.Direction.NatrualValues();
            var animatedDirection = Vector2.zero;
            if (naturalDirection.y > 0 && naturalDirection.y > naturalDirection.x)
            {
                animatedDirection.y = msg.Direction.y;
                animatedDirection.x = 0;
            }
            else if (naturalDirection.x > 0)
            {
                animatedDirection.x = msg.Direction.x;
                animatedDirection.y = 0;
            }
            if (animatedDirection != Vector2.zero)
            {
                if (FlipSpriteX)
                {
                    SpriteRenderer.flipX = animatedDirection.x < 0;
                }
                
                _animator.SetFloat(StaticAnimationParameterStrings.X, animatedDirection.x);
                _animator.SetFloat(StaticAnimationParameterStrings.Y, animatedDirection.y);
            }
        }
    }


    //TODO: Remove when we're sure facing direction is no longer needed - replaced by Aim Direction
    private void SetFacingDirection(SetFacingDirectionMessage msg)
    {
        if (msg.Direction != Vector2.zero && !_locked)
        {
            var naturalDirection = msg.Direction.NatrualValues();
            var animatedDirection = Vector2.zero;
            if (naturalDirection.y > 0 && naturalDirection.y > naturalDirection.x)
            {
                animatedDirection.y = msg.Direction.y;
                animatedDirection.x = 0;
            }
            else if (naturalDirection.x > 0)
            {
                animatedDirection.x = msg.Direction.x;
                animatedDirection.y = 0;
            }
            if (animatedDirection != Vector2.zero)
            {
                if (FlipSpriteX)
                {
                    SpriteRenderer.flipX = animatedDirection.x < 0;
                }
                _animator.SetFloat(StaticAnimationParameterStrings.X, animatedDirection.x);
                _animator.SetFloat(StaticAnimationParameterStrings.Y, animatedDirection.y);
            }
        }
    }

    private void SetUnitAnimationState(SetUnitAnimationStateMessage msg)
    {
        if (_currentAnimationState != msg.State && !_locked)
        {
            _animator.SetFloat(StaticAnimationParameterStrings.UNIT_ANIMATION_STATE, (float)msg.State);
            //TODO: Replace with a string constant
            _animator.Play("Main Tree", 0, 0f);
            _currentAnimationState = msg.State;
            gameObject.SendMessageTo(new UpdateAnimationStateMessage{State = msg.State}, transform.parent.gameObject);
        }
    }

    private void SetUnitRuntimeAnimator(SetUnitRuntimeAnimatorMessage msg)
    {
        _animator.runtimeAnimatorController = msg.Animator;
    }

    private void SetOutline(SetOutlineMessage msg)
    {
        _outline.color = msg.Color;
        _outline.outlineSize = msg.Thickness;
    }

    private void ApplyFlashColorEffect(ApplyFlashColorEffectMessage msg)
    {
        //TODO: Replace with shader when Mesh Renderers are implemented
        if (_flashTween != null)
        {
            _flashTween.Kill(true);
            _flashTween = null;
        }
        var originColor = SpriteRenderer.color;
        var color = originColor;
        var colorTween = ChangeToColor(msg.ChangeToColor, msg.LengthBetweenFlashes);
        for (var i = 1; i < msg.Flashes; i++)
        {
            var previousTween = colorTween;
            colorTween = ChangeToColor(color, msg.LengthBetweenFlashes);
            colorTween.Pause();
            var nextTween = colorTween;
            previousTween.OnComplete(() =>
            {
                nextTween.Play();
            });
            color = color == originColor ? msg.ChangeToColor : originColor;
        }
        colorTween.OnComplete(() =>
        {
            SpriteRenderer.color = originColor;
            _flashTween = null;
        });
        _flashTween = colorTween;
    }

    private void LockAnimationState(LockAnimationStateMessage msg)
    {
        _locked = msg.Locked;
    }

    private void AnimationCheck(AnimationCheckMessage msg)
    {
        if (msg.States.Contains(_currentAnimationState))
        {
            msg.DoAfter?.Invoke();
        }
    }

    private void ChangeSpriteColor(ChangeSpriteColorMessage msg)
    {

        var meshRender = new MeshRenderer();
        var material = SpriteRenderer.materials[1];
        //var materialPropertyBlock = new MaterialPropertyBlock();
        //SpriteRenderer.GetPropertyBlock(materialPropertyBlock);
        material.SetColor("_ColorIn", msg.FromColor);
        material.SetColor("_Color1out", msg.ToColor);
        //SpriteRenderer.SetPropertyBlock(materialPropertyBlock);
        SpriteRenderer.material = material;
    }

}
