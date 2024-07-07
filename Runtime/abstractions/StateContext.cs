using System;

namespace StateMachine.Runtime.abstractions
{
    public interface IStateContext<in TState> where TState : Enum
    {
        void ChangeState(TState newState);
    }

}