using UnityEngine;

public interface ICollectedCards
{
    public Transform CardCollectingArea { get; }
    public int CollectedCardPoints { get; }
    public void CollectCard(ICardBehaviour cardBehaviour);
}