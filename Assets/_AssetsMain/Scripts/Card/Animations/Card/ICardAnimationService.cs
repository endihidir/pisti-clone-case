using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public interface ICardAnimationService
{
    public UniTask Move(Vector3 pos, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public UniTask Rotate(Quaternion rot, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public UniTask Flip(CardFace cardFace, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public void Flip(CardFace cardFace);
    public void Dispose();
}