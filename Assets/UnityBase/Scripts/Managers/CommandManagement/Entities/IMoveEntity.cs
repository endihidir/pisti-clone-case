using UnityEngine;

namespace UnityBase.Command
{
    public interface IMoveEntity
    {
        public Transform HandlerTransform { get; }
        public Transform ObjectTransform { get; }
        public Vector3 TargetPosition { get; }
        public float Speed { get; }
        public bool CanPassNextMovementInstantly { get; }
    }
}