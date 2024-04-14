using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectedCards
{
    public UserBoardView UserBoardView { get; }
    public Transform CardCollectingArea { get; }
    public int CollectedCardCount { get; }
    public int CollectedCardPoint { get; }
    public void CollectCard(ICardBehaviour cardBehaviour);
    public void AddExtraPoint(int point);
    public void UpdateTotalCardPointView();
    public void UpdateCollectedCardCountView();
    public IDictionary<Type, CollectedCardData> GetCollectedCardsData();
    public IDictionary<Type, CollectedCardData> GetPistiCardsData();
    public void Reset();
}