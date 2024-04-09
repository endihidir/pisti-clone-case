using System;
using DG.Tweening;
using UnityEngine;

public class CardAnimationProvider : ICardAnimationService
{
    public ICardAnimation CardAnimation { get; }

    private Transform _moveTransform, _flipTransform;

    private Tween _cardMoveTween, _cardFlipTween;

    public CardAnimationProvider(ICardAnimation cardAnimation)
    {
        CardAnimation = cardAnimation;
        _moveTransform = CardAnimation.CardMoveTransform;
        _flipTransform = CardAnimation.CardFlipTransform;
    }
    
    public void Move(Vector3 pos, float duration, Ease ease, float delay = 0f, Action onComplete = default)
    {
        _cardMoveTween?.Kill();

        _cardMoveTween = _moveTransform.DOMove(pos, duration)
                                   .SetEase(ease)
                                   .SetDelay(delay)
                                   .OnComplete(() => onComplete?.Invoke());
    }

    public void Flip(CardFace cardFace, float duration, Ease ease, float delay = 0f, Action onComplete = default)
    {
        _cardFlipTween?.Kill();

        _cardFlipTween = DOTween.Sequence()
                            .AppendInterval(delay)
                            .Append(_flipTransform.DORotate(Vector3.up * 90f, duration).SetEase(ease))
                            .AppendCallback(()=> CardAnimation.FlipCard(cardFace))
                            .Append(_flipTransform.DORotate(Vector3.up * 0f, duration).SetEase(ease))
                            .AppendCallback(()=> onComplete?.Invoke());
    }

    public void Dispose()
    {
        _cardMoveTween?.Kill();
        _cardFlipTween?.Kill();
    }
}