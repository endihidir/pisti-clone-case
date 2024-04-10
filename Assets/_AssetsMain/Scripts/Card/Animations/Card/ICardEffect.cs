using UnityEngine;

public interface ICardEffect
{
    public Transform CardMoveTransform { get; }
    
    public Transform CardFlipTransform { get; }
    public void FlipCard(CardFace cardFace);
}