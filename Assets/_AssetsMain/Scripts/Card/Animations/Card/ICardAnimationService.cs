using System;
using DG.Tweening;
using UnityEngine;

public interface ICardAnimationService
{
    public ICardAnimation CardAnimation { get; }
    public void Move(Vector3 pos, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public void Flip(CardFace cardFace, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public void Dispose();
}