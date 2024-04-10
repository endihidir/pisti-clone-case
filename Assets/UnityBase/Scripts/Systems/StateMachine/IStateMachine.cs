
namespace UnityBase.StateMachineCore
{
    public interface IStateMachine
    {
        public IState CurrentGameplayState { get; }
        public void ChangeState(IState state);
    }
}
