using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace IKhom.StateMachineSystem.Runtime.helpers
{
    /// <summary>
    /// Utility class for handling asynchronous operations.
    /// </summary>
    internal static class AsyncUtility
    {
        /// <summary>
        /// Runs an asynchronous method synchronously based on the current synchronization context.
        /// </summary>
        /// <param name="asyncMethod">The asynchronous method to run.</param>
        internal static void RunSynchronous(Func<UniTask> asyncMethod)
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