using System;
using System.Linq;
using Sirenix.Utilities;
using UnityBase.Pool;
using UnityEngine;

public class CardViewController : MonoBehaviour, IPoolable, ICardAnimation
{
    [SerializeField] private RectTransform _cardRectTransform;
    [SerializeField] private Transform _cardUIHandlerTransform;
    [SerializeField] private CardView[] _cardViews;
    [SerializeField] private Sprite _cardBackSprite;
    [SerializeField] private CardView _selectedCardView;
    
    private Vector2 _defaultSizeDelta;
    
    private ICardBehaviour _cardBehaviour;
    private ICardAnimationService _cardAnimationService;
    public ICardBehaviour CardBehaviour => _cardBehaviour;

    #region POOL
    public Component PoolableObject => this;
    public bool IsActive => isActiveAndEnabled;
    public bool IsUnique => false;
    
    #endregion

    #region ANIMATION
    public Transform CardMoveTransform => _cardRectTransform;
    public Transform CardFlipTransform => _cardUIHandlerTransform;
    
    #endregion
    

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        _cardRectTransform = GetComponent<RectTransform>();
        _cardViews = GetComponentsInChildren<CardView>(true);
    }
#endif
    
    private void Awake() => _defaultSizeDelta = _cardRectTransform.sizeDelta;
    
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
    
    public void FlipCard(FlipSide flipSide)
    {
        var cardSprite = flipSide == FlipSide.Back ? _cardBackSprite : _cardBehaviour.CardSprite;
        
        _selectedCardView.SetCardSprite(cardSprite);

        if (_selectedCardView is NumberedCardView numberedCardView)
        {
            numberedCardView.EnableText(flipSide == FlipSide.Face);
        }
    }

    public void Initialize(ICardBehaviour cardBehaviour)
    {
        _cardBehaviour = cardBehaviour;
        _cardAnimationService = new CardAnimationProvider(this);
        _cardBehaviour.CardAnimationService = _cardAnimationService;
        
        DisableAllViews();

        if (_cardBehaviour.CardNumber > 0)
        {
            _selectedCardView = GetCardView<NumberedCardView>().SetNumber(_cardBehaviour.CardNumber);
        }
        else
        {
            _selectedCardView = GetCardView<SpecialCardView>();
        }

        _selectedCardView.SetCardSprite(_cardBehaviour.CardSprite);
        EnableCardView(_selectedCardView, true);
    }

    private T GetCardView<T>() where T : CardView => _cardViews.FirstOrDefault(x => x is T) as T;
    private void EnableCardView(CardView cardView, bool enable) => cardView.SetActive(enable);
    private void DisableAllViews() => _cardViews.ForEach(cardView => EnableCardView(cardView, false));
    private void OnDestroy() => _cardAnimationService?.Dispose();
}