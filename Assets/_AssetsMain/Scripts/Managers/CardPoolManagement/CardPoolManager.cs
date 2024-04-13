using System;
using UnityBase.ManagerSO;
using UnityBase.Service;
using UnityEngine;

public class CardPoolManager : ICardPoolService, IGameplayConstructorService
{
    private readonly IPoolDataService _poolDataService;
    
    private Transform _cardViewParent;

    public CardPoolManager(GameplayDataHolderSO gameplayDataHolderSo, IPoolDataService poolDataService)
    {
        _cardViewParent = gameplayDataHolderSo.cardPoolManagerSo.cardOnScreenParent;
        
        _poolDataService = poolDataService;
    }
    public void Initialize() { }
    public void Dispose() { }

    public T GetCardView<T>(bool show = true, float duration = 0f, float delay = 0) where T : CardViewController
    {
        var cardView = _poolDataService.GetObject<T>(show, duration, delay);
        cardView.transform.SetParent(_cardViewParent);
        cardView.ResetCardViewSize();
        cardView.transform.localPosition = Vector3.zero;
        return cardView;
    }

    public void HideCardView(CardViewController cardViewController, float duration = 0f, float delay = 0f, Action onComplete = default, bool readLogs = false)
    {
        _poolDataService.HideObject(cardViewController, duration, delay, onComplete, readLogs);
    }

    public void HideAllCardViewOfType<T>(float duration = 0f, float delay = 0f, Action onComplete = default) where T : CardViewController
    {
        _poolDataService.HideAllObjectsOfType<T>(duration, delay, onComplete);
    }

    public void HideAllCardView(float duration = 0f, float delay = 0f)
    {
        _poolDataService.HideAllObjectsOfType<CardViewController>(duration, delay);
    }

    public void RemoveCardViewPool<T>(bool readLogs = false) where T : CardViewController
    {
        _poolDataService.RemovePool<T>(readLogs);
    }
}