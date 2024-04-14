using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCardsContainer : ICollectedCards
{
    private readonly UserBoardView _userBoardView;
    private readonly IDictionary<Type, CollectedCardData> _cardsPoints;
    private readonly IDictionary<Type, CollectedCardData> _pistiPoints;
    private readonly Transform _cardCollectingArea;
    
    private int _collectedTotalPoint;
    private int _collectedCardCount;
    private int _collectedCardCounterForView;

    public UserBoardView UserBoardView => _userBoardView;
    public Transform CardCollectingArea => _cardCollectingArea;
    public int CollectedCardPoint => _collectedTotalPoint;
    public int CollectedCardCount => _collectedCardCount;

    public CollectedCardsContainer(UserBoardView userBoardView)
    {
        _userBoardView = userBoardView;
        _cardCollectingArea = _userBoardView.CardCollectingArea;

        _cardsPoints = new Dictionary<Type, CollectedCardData>();
        _pistiPoints = new Dictionary<Type, CollectedCardData>();
    }

    public void CollectCard(ICardBehaviour cardBehaviour)
    {
        _collectedCardCount++;
        
        FillCardPointData(cardBehaviour);
        
        FillPistiCardPointData(cardBehaviour);
    }

    private void FillCardPointData(ICardBehaviour cardBehaviour)
    {
        if (cardBehaviour.GetCardPoint() <= 0) return;
        
        var point = cardBehaviour.GetCardPoint();

        var cardType = cardBehaviour.GetType();

        if (!_cardsPoints.ContainsKey(cardType))
        {
            _cardsPoints[cardType] = new CollectedCardData { cardBehaviour = cardBehaviour, totalCardPoint = point };
        }
        else
        {
            var data = _cardsPoints[cardType];
            data.totalCardPoint += point;
            _cardsPoints[cardType] = data;
        }

        _collectedTotalPoint += point;
    }

    private void FillPistiCardPointData(ICardBehaviour cardBehaviour)
    {
        var pistiPoint = (cardBehaviour.IsPistiCard ? cardBehaviour is JackCard ? 20 : 10 : 0);

        if (pistiPoint <= 0) return;
        
        var cardType = cardBehaviour.GetType();

        if (!_pistiPoints.ContainsKey(cardType))
        {
            _pistiPoints[cardType] = new CollectedCardData { cardBehaviour = cardBehaviour, totalCardPoint = pistiPoint };
        }
        else
        {
            var data = _pistiPoints[cardType];
            data.totalCardPoint += pistiPoint;
            _pistiPoints[cardType] = data;
        }

        _collectedTotalPoint += pistiPoint;
    }

    public void UpdateTotalCardPointView() => _userBoardView.SetScore(_collectedTotalPoint);

    public void UpdateCollectedCardCountView()
    {
        _collectedCardCounterForView++;
        _userBoardView.SetCollectedCardCount(_collectedCardCounterForView);
    }

    public void AddExtraPoint(int point)
    {
        _collectedTotalPoint += point;
        UpdateTotalCardPointView();
    }

    public IDictionary<Type, CollectedCardData> GetCollectedCardsData() => _cardsPoints;
    public IDictionary<Type, CollectedCardData> GetPistiCardsData() => _pistiPoints;
    public void Reset()
    {
        _collectedCardCounterForView = 0;
        _collectedCardCount = 0;
        _cardsPoints.Clear();
        _pistiPoints.Clear();
    }
}

public struct CollectedCardData
{
    public ICardBehaviour cardBehaviour;
    public int totalCardPoint;
}