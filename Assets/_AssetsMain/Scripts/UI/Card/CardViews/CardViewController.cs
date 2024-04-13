using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityBase.Pool;
using UnityEngine;

public class CardViewController : MonoBehaviour, IPoolable, ICardAnimation, ICardInputDetector
{
    [SerializeField] private RectTransform _cardRectTransform;
    [SerializeField] private Transform _cardFlipTrinsform;
    [SerializeField] private CardAtlasSO _cardAtlas;
    [SerializeField] private CardView[] _cardViews;
    [ReadOnly, SerializeField] private CardView _selectedCardView;
    
    private Sprite _frontFaceSprite;
    private Vector2 _defaultSizeDelta;
    private ICardAnimationService _cardAnimationService;
    private ICardInputDetectionService _cardInputDetectionService;
    
    public Component PoolableObject => this;
    public bool IsActive => isActiveAndEnabled;
    public bool IsUnique => false;

    public Transform CardMoveTransform => transform;
    public Transform CardFlipTransform => _cardFlipTrinsform;
    public CardView SelectedCardView => _selectedCardView;
    public Sprite CardFrontFace => _frontFaceSprite;
    public Sprite CardBackFace => _cardAtlas.GetSprite(CardType.Card_Back_Face);

    private void Awake()
    {
        _defaultSizeDelta = _cardRectTransform.sizeDelta;
    }

    public void Initialize(ICardBehaviour cardBehaviour)
    {
        DisableAllViews();
        
        _selectedCardView = cardBehaviour is NumberedCard ? GetCardView<NumberedCardView>().SetNumber(cardBehaviour.CardNumber) : GetCardView<SpecialCardView>();
        _frontFaceSprite = _cardAtlas.GetSprite(cardBehaviour.CardType);
        
        _cardAnimationService = new CardAnimationProvider(this);
        cardBehaviour.CardAnimationService = _cardAnimationService;
        cardBehaviour.CardAnimationService.Flip(CardFace.Back);
        
        _cardInputDetectionService = new CardInputDetectionProvider(this);
        cardBehaviour.CardInputDetectionService = _cardInputDetectionService;
        
        EnableCardView(_selectedCardView, true);
    }
    
    public void ResetCardViewSize()
    {
        _cardRectTransform.sizeDelta = _defaultSizeDelta;
        transform.localScale = Vector3.one;
    }
    
    public void Show(float duration, float delay, Action onComplete)
    {
        gameObject.SetActive(true);
        onComplete?.Invoke();
    }
    
    public void Hide(float duration, float delay, Action onComplete)
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private T GetCardView<T>() where T : CardView => _cardViews.FirstOrDefault(x => x is T) as T;
    private void EnableCardView(CardView cardView, bool enable) => cardView.SetActive(enable);
    private void DisableAllViews() => _cardViews.ForEach(cardView => EnableCardView(cardView, false));
    private void OnDestroy() => _cardAnimationService?.Dispose();
}