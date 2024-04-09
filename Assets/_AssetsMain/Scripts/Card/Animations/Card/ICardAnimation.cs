using UnityEngine;

public interface ICardAnimation
{
    public Transform CardMoveTransform { get; }
    public Transform CardFlipTransform { get; }
    public void FlipCard(CardFace cardFace);
}