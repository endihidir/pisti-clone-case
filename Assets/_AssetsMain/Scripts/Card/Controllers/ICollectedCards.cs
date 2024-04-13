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
    public IDictionary<Type, CollectedCardData> GetSumOfCollectedCards();
    public IDictionary<Type, CollectedCardData> GetSumOfPistiCards();
    public void Reset();
}