using System;

namespace IKhom.StateMachineSystem.Runtime.helpers
{
    /// <summary>
    /// Custom exception for state machine-specific errors.
    /// </summary>
    internal class StateMachineException : Exception
    {
        public StateMachineException()
        {
        }

        public StateMachineException(string message) : base(message)
        {
        }

        public StateMachineException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}