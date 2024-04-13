using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public interface ICardAnimationService
{
    public UniTask Move(Vector3 targetPosition, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public UniTask MoveAdditive(Vector3 targetPosition, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public UniTask Rotate(Quaternion targetRotation, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public UniTask Flip(CardFace cardFace, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public UniTask PistiAnim(float zRotAngle, float duration, Ease ease, float delay = 0f, Action onComplete = default);
    public void Flip(CardFace cardFace);
    public void Dispose();
}