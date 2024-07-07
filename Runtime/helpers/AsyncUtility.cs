using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace StateMachine.Runtime.helpers
{
    /// <summary>
    /// Utility class for handling asynchronous operations.
    /// </summary>
    public static class AsyncUtility
    {
        /// <summary>
        /// Runs an asynchronous method synchronously based on the current synchronization context.
        /// </summary>
        /// <param name="asyncMethod">The asynchronous method to run.</param>
        public static void RunSynchronous(Func<UniTask> asyncMethod)
        {
            if (SynchronizationContext.Current == null)
            {
                asyncMethod().GetAwaiter().GetResult();
            }
            else
            {
                asyncMethod().Forget();
            }
        }
    }
}