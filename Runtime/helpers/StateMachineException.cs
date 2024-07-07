using System;

namespace Utilities.StateMachine.helpers
{
    /// <summary>
    /// Custom exception for state machine-specific errors.
    /// </summary>
    public class StateMachineException : Exception
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