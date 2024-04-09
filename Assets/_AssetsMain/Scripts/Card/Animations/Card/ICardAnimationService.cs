using System;
using DG.Tweening;
using UnityEngine;

public interface ICardAnimationService
{
    public ICardAnimation CardAnimation { get; }
    public void Move(Vector3 pos, float duration, Ease ease, Action onComplete, float delay = 0f);
    public void Flip(FlipSide flipSide, float duration, Ease ease, Action onComplete, float delay = 0f);
    public void Dispose();
}