using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CardAnimationProvider : ICardAnimationService
{
    private readonly ICardAnimation _cardAnimation;
    private readonly Transform _moveTransform, _flipTransform;
    private readonly CardView _selectedCardView;
    private readonly Sprite _cardBackFace, _cardFrontFace;
    private CardFace _currentCardFace;
    
    private Tween _cardMoveTween, _cardFlipTween, _cardRotateTween, _pistiAnimTween;

    public CardAnimationProvider(ICardAnimation cardAnimation)
    {
        _cardAnimation = cardAnimation;
        _moveTransform = _cardAnimation.CardMoveTransform;
        _flipTransform = _cardAnimation.CardFlipTransform;
        _selectedCardView = _cardAnimation.SelectedCardView;
        _cardFrontFace = _cardAnimation.CardFrontFace;
        _cardBackFace = _cardAnimation.CardBackFace;
    }

    public async UniTask Move(Vector3 targetPosition, float duration, Ease ease, float delay = 0)
    {
        _cardMoveTween?.Kill();

        var isSiblingOrderChanged = false;
        
        var halfOfStartDistance = GetHalfOfStartDistance(_moveTransform.position, targetPosition);

        _cardMoveTween = _moveTransform.DOMove(targetPosition, duration)
                                       .OnUpdate(() => ChangeSiblingOrder(targetPosition, halfOfStartDistance, ref isSiblingOrderChanged))
                                       .SetEase(ease)
                                       .SetDelay(delay);

        
        await _cardMoveTween.AsyncWaitForCompletion().AsUniTask();
    }

    public async UniTask MoveAdditive(Vector3 targetPosition, float duration, Ease ease, float delay = 0)
    {
        _cardMoveTween?.Kill();
        
        var startPos = _moveTransform.position;
        
        _cardMoveTween = _moveTransform.DOMove(startPos + targetPosition, duration)
                                       .SetEase(ease)
                                       .SetDelay(delay);
        
        await _cardMoveTween.AsyncWaitForCompletion().AsUniTask();
    }

    public async UniTask Rotate(Quaternion targetRotation, float duration, Ease ease, float delay = 0)
    {
        _cardRotateTween?.Kill();
        
        _cardRotateTween = _moveTransform.DORotateQuaternion(targetRotation, duration)
                                         .SetEase(ease)
                                         .SetDelay(delay);
        
        await _cardRotateTween.AsyncWaitForCompletion().AsUniTask();
    }

    public async UniTask Flip(CardFace cardFace, float duration, Ease ease, float delay = 0f)
    {
        if(_currentCardFace == cardFace) return;

        _cardFlipTween?.Kill();

        _cardFlipTween = DOTween.Sequence()
                                .AppendInterval(delay)
                                .Append(_flipTransform.DOLocalRotate(Vector3.up * 90f, duration * 0.5f).SetEase(ease))
                                .AppendCallback(() => Flip(cardFace))
                                .Append(_flipTransform.DOLocalRotate(Vector3.up * 0f, duration * 0.5f).SetEase(ease));

        await _cardFlipTween.AsyncWaitForCompletion().AsUniTask();
    }
    
    public async UniTask PistiAnim(float zRotAngle, float duration, Ease ease, float delay = 0f)
    {
        _pistiAnimTween?.Kill();

        _pistiAnimTween = _flipTransform.DOLocalRotate(Vector3.forward * zRotAngle, duration)
                                        .SetEase(ease)
                                        .SetDelay(delay);
        
        await _pistiAnimTween.AsyncWaitForCompletion().AsUniTask();
    }

    public void Flip(CardFace cardFace)
    {
        _currentCardFace = cardFace;
        
        var cardSprite = cardFace == CardFace.Back ? _cardBackFace : _cardFrontFace;
        
        _selectedCardView.SetCardSprite(cardSprite);

        if (_selectedCardView is NumberedCardView numberedCardView)
        {
            numberedCardView.EnableText(cardFace == CardFace.Front);
        }
    }

    private float GetHalfOfStartDistance(Vector3 startPosition, Vector3 targetPosition)
    {
        var startDistance = Vector3.Distance(startPosition, targetPosition);
        return startDistance * 0.5f;
    }
    
    private void ChangeSiblingOrder(Vector3 targetPosition, float halfOfStartDistance, ref bool isSiblingOrderChanged)
    {
        if(isSiblingOrderChanged) return;
        
        var currentDistance = Vector3.Distance(_moveTransform.position, targetPosition);

        if (currentDistance < halfOfStartDistance)
        {
            isSiblingOrderChanged = true;
            
            _moveTransform.SetAsLastSibling();
        }
    }

    public void Dispose()
    {
        _cardRotateTween?.Kill();
        _cardMoveTween?.Kill();
        _cardFlipTween?.Kill();
    }
}