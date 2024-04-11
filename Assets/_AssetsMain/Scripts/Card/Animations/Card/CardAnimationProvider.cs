using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CardAnimationProvider : ICardAnimationService
{
    private readonly ICardAnimation _cardAnimation;

    private Transform _moveTransform, _flipTransform;
    private Tween _cardMoveTween, _cardFlipTween, _cardRotateTween, _siblingChangeDelayTween;

    public CardAnimationProvider(ICardAnimation cardAnimation)
    {
        _cardAnimation = cardAnimation;
        _moveTransform = _cardAnimation.CardMoveTransform;
        _flipTransform = _cardAnimation.CardFlipTransform;
    }

    public async UniTask Move(Vector3 pos, float duration, Ease ease, float delay = 0, Action onComplete = default)
    {
        _cardMoveTween?.Kill();
        
        _siblingChangeDelayTween = DOVirtual.DelayedCall(duration * 0.5f, () => _moveTransform.SetAsLastSibling());
        
        _cardMoveTween = _moveTransform.DOMove(pos, duration)
                                       .SetEase(ease)
                                       .SetDelay(delay)
                                       .OnComplete(() => onComplete?.Invoke());

        
        await _cardMoveTween.AsyncWaitForCompletion().AsUniTask();
    }

    public async UniTask Rotate(Quaternion rot, float duration, Ease ease, float delay = 0, Action onComplete = default)
    {
        _cardRotateTween?.Kill();
        
        _cardRotateTween = _moveTransform.DORotateQuaternion(rot, duration)
                                       .SetEase(ease)
                                       .SetDelay(delay)
                                       .OnComplete(() => onComplete?.Invoke());
        
        await _cardRotateTween.AsyncWaitForCompletion().AsUniTask();
    }

    public async UniTask Flip(CardFace cardFace, float duration, Ease ease, float delay = 0f, Action onComplete = default)
    {
        _cardFlipTween?.Kill();

        _cardFlipTween = DOTween.Sequence()
                            .AppendInterval(delay)
                            .Append(_flipTransform.DOLocalRotate(Vector3.up * 90f, duration * 0.5f).SetEase(ease))
                            .AppendCallback(()=> Flip(cardFace))
                            .Append(_flipTransform.DOLocalRotate(Vector3.up * 0f, duration * 0.5f).SetEase(ease))
                            .AppendCallback(()=> onComplete?.Invoke());
        
        await _cardFlipTween.AsyncWaitForCompletion().AsUniTask();
    }

    public void Flip(CardFace cardFace) => _cardAnimation.FlipCard(cardFace);

    public void Dispose()
    {
        _cardRotateTween?.Kill();
        _cardMoveTween?.Kill();
        _cardFlipTween?.Kill();
        _siblingChangeDelayTween?.Kill();
    }
}