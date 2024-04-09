using System;
using DG.Tweening;
using UnityEngine;

public class CardAnimationProvider : ICardAnimationService
{
    public ICardAnimation CardAnimation { get; }

    private Transform _moveTransform, _flipTransform;

    private Tween _cardTween;

    public CardAnimationProvider(ICardAnimation cardAnimation)
    {
        CardAnimation = cardAnimation;

        _moveTransform = CardAnimation.CardMoveTransform;
        
        _flipTransform = CardAnimation.CardFlipTransform;
    }
    
    public void Move(Vector3 pos, float duration, Ease ease, Action onComplete, float delay = 0)
    {
        
    }

    public void Flip(FlipSide flipSide, float duration, Ease ease, Action onComplete, float delay = 0)
    {
        _cardTween?.Kill();

        _cardTween = DOTween.Sequence()
                            .AppendInterval(delay)
                            .Append(_flipTransform.DORotate(Vector3.up * 90f, duration).SetEase(ease))
                            .AppendCallback(()=> CardAnimation.FlipCard(flipSide))
                            .Append(_flipTransform.DORotate(Vector3.up * 0f, duration).SetEase(ease))
                            .AppendCallback(()=> onComplete?.Invoke());
    }

    public void Dispose() => _cardTween?.Kill();
}