using Assets.Scripts.Animation;
using Assets.Scripts.System;
using DG.Tweening;
using MessageBusLib;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    [Header("Settings")]
    public bool FlipSpriteX;

    public SpriteRenderer SpriteRenderer { get; set; }

    private Animator _animator { get; set; }
    private UnitAnimationState _currentAnimationState { get; set; }
    private SpriteOutline _outline { get; set; }
    private Tween _flashTween { get; set; }
    private bool _locked { get; set; }

    void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        SpriteRenderer = transform.GetComponent<SpriteRenderer>();
        _outline = transform.GetComponent<SpriteOutline>();
        if (_animator && SpriteRenderer)
        {
            SubscribeToMessages();
        }
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
        //transform.parent.gameObject.Subscribe<SetFacingDirectionMessage>(SetFacingDirection);
        transform.parent.gameObject.Subscribe<SetUnitAnimationStateMessage>(SetUnitAnimationState);
        transform.parent.gameObject.Subscribe<SetUnitRuntimeAnimatorMessage>(SetUnitRuntimeAnimator);
        transform.parent.gameObject.Subscribe<AimDirectionMessage>(AimDirection);
        transform.parent.gameObject.Subscribe<SetOutlineMessage>(SetOutline);
        transform.parent.gameObject.Subscribe<ApplyFlashColorEffectMessage>(ApplyFlashColorEffect);
        transform.parent.gameObject.Subscribe<LockAnimationStateMessage>(LockAnimationState);
    }

    private Tween ChangeToColor(Color color, float time)
    {
        return SpriteRenderer.DOColor(color, time).SetEase(Ease.Linear);
    }

    private void AimDirection(AimDirectionMessage msg)
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


}
