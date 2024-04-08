using System;
using System.Linq;
using Sirenix.Utilities;
using UnityBase.Pool;
using UnityEngine;

public class CardViewController : MonoBehaviour, IPoolable
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Transform _cardUIHandler;
    [SerializeField] private CardView[] _cardViews;
    
    private ICardBehaviour _cardBehaviour;
    
    private Vector2 _defaultSizeDelta;
    
    public Component PoolableObject => this;
    public bool IsActive => isActiveAndEnabled;
    public bool IsUnique => false;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        _cardViews = GetComponentsInChildren<CardView>(true);
        _rectTransform = GetComponent<RectTransform>();
    }
#endif
    
    private void Awake()
    {
        _defaultSizeDelta = _rectTransform.sizeDelta;
    }

    public void ResetCardViewSize()
    {
        _rectTransform.sizeDelta = _defaultSizeDelta;
        transform.localScale = Vector3.one;
    }
    
    public void Show(float duration, float delay, Action onComplete)
    {
        gameObject.SetActive(true);

        onComplete.Invoke();
    }
    
    public void Hide(float duration, float delay, Action onComplete)
    {
        gameObject.SetActive(false);
        
        onComplete?.Invoke();
    }

    public void Initialize(ICardBehaviour cardBehaviour)
    {
        _cardBehaviour = cardBehaviour;

        _cardViews.ForEach(x => x.gameObject.SetActive(false));

        if (_cardBehaviour.CardNumber > 0)
        {
            var numberedCardView = _cardViews.FirstOrDefault(x => x is NumberedCardView) as NumberedCardView;

            if (!numberedCardView) return;
            
            numberedCardView.SetNumber(_cardBehaviour.CardNumber);
            
            numberedCardView.SetCardSprite(_cardBehaviour.CardSprite);
            
            numberedCardView.gameObject.SetActive(true);
        }
        else
        {
            var specialCardView = _cardViews.FirstOrDefault(x => x is SpecialCardView) as SpecialCardView;
            
            if(!specialCardView) return;
            
            specialCardView.SetCardSprite(_cardBehaviour.CardSprite);
            
            specialCardView.gameObject.SetActive(true);
        }
    }
}