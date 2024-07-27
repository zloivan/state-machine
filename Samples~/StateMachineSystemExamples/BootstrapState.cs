using System.Threading;
using Cysharp.Threading.Tasks;
using IKhom.StateMachineSystem.Runtime.abstractions;
using UnityEngine;

namespace IKhom.StateMachineSystem.Samples.StateMachineSystemExamples
{
    public class BootstrapState : IState<AppState>
    {
        private readonly IStateContext<AppState> _context;

        public BootstrapState(IStateContext<AppState> context)
        {
            _context = context;
        }

        public async UniTask EnterAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("Entering Bootstrap State");
            await UniTask.Delay(2000, cancellationToken: cancellationToken);
            Debug.Log("Bootstrap data loaded");

            _context.ChangeState(AppState.MainMenu);
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