using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities.StateMachine.abstractions;

namespace Utilities.StateMachine.Examples
{
    public class BootstrapState : IState<AppState>
    {
        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Bootstrap State");
            await UniTask.Delay(2000, cancellationToken: cancellationToken);
            Debug.Log("Bootstrap data loaded");
        }

        public UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Exiting Bootstrap State");
            return UniTask.CompletedTask;
        }
    }
}