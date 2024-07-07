using UnityEngine;

namespace StateMachine.Runtime.helpers
{
    /// <summary>
    /// Logger class for the state machine.
    /// </summary>
    public class StateMachineLogger
    {
        public void Log(string message)
        {
#if DEBUG && DBUG
            Debug.Log($"[StateMachine] {message}");
#endif
        }

        public void LogWarning(string message)
        {
#if DEBUG || UNITY_EDITOR
            Debug.LogWarning($"[StateMachine] {message}");
#endif
        }

        public void LogError(string message)
        {
#if DEBUG || UNITY_EDITOR
            Debug.LogError($"[StateMachine] {message}");
#endif
        }
    }
}