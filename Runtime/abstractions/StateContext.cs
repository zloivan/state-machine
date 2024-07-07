using System;

namespace Utilities.StateMachine.abstractions
{
    public interface IStateContext<in TState> where TState : Enum
    {
        void ChangeState(TState newState);
    }

}