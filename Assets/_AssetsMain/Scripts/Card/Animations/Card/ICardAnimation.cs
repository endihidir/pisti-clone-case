using UnityEngine;

public interface ICardAnimation
{
    public Transform CardMoveTransform { get; }
    public Transform CardFlipTransform { get; }
    public CardView SelectedCardView { get; }
    public Sprite CardFrontFace { get; }
    public Sprite CardBackFace { get; }
}