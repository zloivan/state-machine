using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace StateMachine.Runtime.abstractions
{
    public interface IState<TState> where TState : Enum
    {
        UniTask EnterAsync(CancellationToken cancellationToken = default);
        UniTask UpdateAsync(CancellationToken cancellationToken = default);
        UniTask ExitAsync(CancellationToken cancellationToken = default);
    }
}