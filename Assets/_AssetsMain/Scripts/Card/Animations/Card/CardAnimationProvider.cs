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
    
    private Tween _cardMoveTween, _cardFlipTween, _cardRotateTween;

    public CardAnimationProvider(ICardAnimation cardAnimation)
    {
        _cardAnimation = cardAnimation;
        _moveTransform = _cardAnimation.CardMoveTransform;
        _flipTransform = _cardAnimation.CardFlipTransform;
        _selectedCardView = _cardAnimation.SelectedCardView;
        _cardFrontFace = _cardAnimation.CardFrontFace;
        _cardBackFace = _cardAnimation.CardBackFace;
    }

    public async UniTask Move(Vector3 pos, float duration, Ease ease, float delay = 0)
    {
        _cardMoveTween?.Kill();

        var isSetToLastSibling = false;
        
        var startDist = Vector3.Distance(_moveTransform.position, pos);
        
        _cardMoveTween = _moveTransform.DOMove(pos, duration)
                                       .OnUpdate(() => ChnageSiblingOrder(pos, startDist, ref isSetToLastSibling))
                                       .SetEase(ease)
                                       .SetDelay(delay);

        
        await _cardMoveTween.AsyncWaitForCompletion().AsUniTask();
    }

    public async UniTask Rotate(Quaternion rot, float duration, Ease ease, float delay = 0)
    {
        _cardRotateTween?.Kill();
        
        _cardRotateTween = _moveTransform.DORotateQuaternion(rot, duration)
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
    
    private void ChnageSiblingOrder(Vector3 pos, float startDist, ref bool isSetToLastSibling)
    {
        var currentDistance = Vector3.Distance(_moveTransform.position, pos);
        if (!(currentDistance < startDist * 0.5f) || isSetToLastSibling) return;
        isSetToLastSibling = true;
        _moveTransform.SetAsLastSibling();
    }

    public void Dispose()
    {
        _cardRotateTween?.Kill();
        _cardMoveTween?.Kill();
        _cardFlipTween?.Kill();
    }
}