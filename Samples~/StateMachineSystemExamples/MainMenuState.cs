using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace IKhom.StateMachineSystem.Samples.StateMachineSystemExamples
{
    public class MainMenuState : IState<AppState>
    {
        public UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Main Menu State");
            return UniTask.CompletedTask;
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Exiting Main Menu State");
            return UniTask.CompletedTask;
        }
    }
}