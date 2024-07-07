using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities.StateMachine.abstractions;

namespace Utilities.StateMachine.Examples
{
    public class GameLoopState : IState<AppState>
    {
        private readonly IStateContext<AppState> _context;

        public GameLoopState(IStateContext<AppState> context)
        {
            _context = context;
        }

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