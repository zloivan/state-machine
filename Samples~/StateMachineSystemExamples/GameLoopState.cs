using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace IKhom.StateMachineSystem.Samples.StateMachineSystemExamples
{
    public class GameLoopState : IState<AppState>
    {
        public UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Game Loop State");
            return UniTask.CompletedTask;
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Exiting Game Loop State");
            return UniTask.CompletedTask;
        }
    }
}