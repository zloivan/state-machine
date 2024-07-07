using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities.StateMachine.abstractions;

namespace Utilities.StateMachine.Examples
{
    public class MainMenuState : IState<AppState>
    {
        
        private readonly IStateContext<AppState> _context;

        public MainMenuState(IStateContext<AppState> context)
        {
            _context = context;
        }

        public MainMenuState()
        {
            
        }
        
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