using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCardsContainer : ICollectedCards
{
    private readonly IDictionary<Type, CollectedCardData> _cardsPoints;
    private readonly IDictionary<Type, CollectedCardData> _pistiPoints;
    private readonly Transform _cardCollectingArea;
    private int _collectedTotalPoint;
    private int _collectedCardCount;
    
    public Transform CardCollectingArea => _cardCollectingArea;
    public int CollectedCardPoint => _collectedTotalPoint;
    public int CollectedCardCount => _collectedCardCount;

    public CollectedCardsContainer(Transform cardCollectingArea)
    {
        _cardCollectingArea = cardCollectingArea;

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

    public void AddExtraPoint(int point) => _collectedTotalPoint += point;
    public IDictionary<Type, CollectedCardData> GetSumOfCollectedCards() => _cardsPoints;
    public IDictionary<Type, CollectedCardData> GetSumOfPistiCards() => _pistiPoints;
    public void Reset()
    {
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