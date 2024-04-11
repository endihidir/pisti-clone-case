using UnityEngine;

public interface ICollectedCards
{
    public Transform CollectedCardPoint { get; }
    public void CollectCard(ICardBehaviour cardBehaviour);
}