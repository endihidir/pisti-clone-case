using UnityEngine;

public class CollectedCardsContainer : ICollectedCards
{
    private readonly Transform _collectedCards;
    public Transform CollectedCardPoint => _collectedCards;

    private int _collectedPoints; 
    
    public CollectedCardsContainer(Transform collectedCards) => _collectedCards = collectedCards;

    public void CollectCard(ICardBehaviour cardBehaviour)
    {
        var pistiPoint = (cardBehaviour.IsPistiCard ? cardBehaviour is JackCard ? 20 : 10 : 0);
        
        _collectedPoints += cardBehaviour.GetCardPoint() + pistiPoint;
    }
}