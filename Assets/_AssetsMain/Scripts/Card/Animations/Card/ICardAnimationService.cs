using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public interface ICardAnimationService
{
    public UniTask Move(Vector3 targetPosition, float duration, Ease ease, float delay = 0f);
    public UniTask Rotate(Quaternion targetRotation, float duration, Ease ease, float delay = 0f);
    public UniTask Flip(CardFace cardFace, float duration, Ease ease, float delay = 0f);
    public UniTask PistiAnim(float zRotAngle, float duration, Ease ease, float delay = 0f);
    public void Flip(CardFace cardFace);
    public void Dispose();
}