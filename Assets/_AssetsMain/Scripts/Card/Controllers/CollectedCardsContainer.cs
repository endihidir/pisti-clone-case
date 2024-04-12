using UnityEngine;

public class CollectedCardsContainer : ICollectedCards
{
    private readonly Transform _cardCollectingArea;
    public Transform CardCollectingArea => _cardCollectingArea;

    private int _collectedPoints; 
    public int CollectedCardPoints => _collectedPoints;
    
    public CollectedCardsContainer(Transform cardCollectingArea) => _cardCollectingArea = cardCollectingArea;

    public void CollectCard(ICardBehaviour cardBehaviour)
    {
        var pistiPoint = (cardBehaviour.IsPistiCard ? cardBehaviour is JackCard ? 20 : 10 : 0);
        
        _collectedPoints += cardBehaviour.GetCardPoint() + pistiPoint;
    }

}