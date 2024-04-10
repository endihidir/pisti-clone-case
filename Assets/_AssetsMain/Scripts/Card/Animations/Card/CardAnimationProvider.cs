using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CardAnimationProvider : ICardAnimationService
{
    private readonly ICardEffect _cardEffect;

    private Transform _moveTransform, _flipTransform;
    private Tween _cardMoveTween, _cardFlipTween, _cardRotateTween;

    public CardAnimationProvider(ICardEffect cardEffect)
    {
        _cardEffect = cardEffect;
        _moveTransform = _cardEffect.CardMoveTransform;
        _flipTransform = _cardEffect.CardFlipTransform;
    }

    public async UniTask Move(Vector3 pos, float duration, Ease ease, float delay = 0, Action onComplete = default)
    {
        _cardMoveTween?.Kill();
        
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
                            .Append(_flipTransform.DOLocalRotate(Vector3.up * 90f, duration).SetEase(ease))
                            .AppendCallback(()=> Flip(cardFace))
                            .Append(_flipTransform.DOLocalRotate(Vector3.up * 0f, duration).SetEase(ease))
                            .AppendCallback(()=> onComplete?.Invoke());
        
        await _cardFlipTween.AsyncWaitForCompletion().AsUniTask();
    }

    public void Flip(CardFace cardFace) => _cardEffect.FlipCard(cardFace);

    public void Dispose()
    {
        _cardRotateTween?.Kill();
        _cardMoveTween?.Kill();
        _cardFlipTween?.Kill();
    }
}