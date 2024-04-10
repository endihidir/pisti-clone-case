using System;

namespace UnityBase.StateMachineCore
{
    public interface IState
    {
        public event Action OnStateComplete;
        public void OnEnter();
        public void OnUpdate(float deltaTime);
        public void OnExit();
        public void Reset();
    }
}