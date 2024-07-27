using System;

namespace IKhom.StateMachineSystem.Runtime.abstractions
{
    public interface IStateContext<in TState> where TState : Enum
    {
        void ChangeState(TState newState);
    }

}