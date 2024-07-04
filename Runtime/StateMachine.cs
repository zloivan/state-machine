using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities.StateMachine.abstractions;

namespace Utilities.StateMachine
{
    public class StateMachine<TState> where TState : Enum
    {
        private TState _currentState;
        private readonly Dictionary<TState, IState<TState>> _states = new();
        private readonly Stack<TState> _stateHistory = new();
        private readonly Dictionary<(TState, TState), Func<bool>> _transitionGuards = new();

        public TState CurrentState => _currentState;

        public event Action<TState, TState> StateChanged;

        public void AddState<TStateImpl>(TState stateKey) where TStateImpl : IState<TState>, new()
        {
            if (_states.ContainsKey(stateKey))
            {
                Debug.LogWarning($"State {stateKey} already exists in the state machine. Overwriting.");
            }
            _states[stateKey] = new TStateImpl();
        }

        public void AddTransition(TState from, TState to, Func<bool> guard)
        {
            _transitionGuards[(from, to)] = guard;
        }

        public void SetInitialState(TState initialState)
        {
            if (SynchronizationContext.Current == null)
            {
                SetInitialStateAsync(initialState).GetAwaiter().GetResult();
            }
            else
            {
                SetInitialStateAsync(initialState).Forget();
            }
        }

        public async UniTask SetInitialStateAsync(TState initialState, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!_states.ContainsKey(initialState))
                {
                    throw new ArgumentException($"State {initialState} does not exist in the state machine.");
                }
                _currentState = initialState;
                _stateHistory.Clear();
                await _states[_currentState].EnterAsync(cancellationToken);
                OnStateChanged(default, _currentState);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error setting initial state: {ex.Message}");
                throw;
            }
        }

        public void ChangeState(TState newState)
        {
            if (SynchronizationContext.Current == null)
            {
                ChangeStateAsync(newState).GetAwaiter().GetResult();
            }
            else
            {
                ChangeStateAsync(newState).Forget();
            }
        }

        public async UniTask ChangeStateAsync(TState newState, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!_states.ContainsKey(newState))
                {
                    throw new ArgumentException($"State {newState} does not exist in the state machine.");
                }

                if (_transitionGuards.TryGetValue((_currentState, newState), out var guard) && !guard())
                {
                    Debug.LogWarning($"Transition from {_currentState} to {newState} is not allowed by the guard.");
                    return;
                }

                var oldState = _currentState;
                await _states[_currentState].ExitAsync(cancellationToken);
                _stateHistory.Push(_currentState);
                _currentState = newState;
                await _states[_currentState].EnterAsync(cancellationToken);
                OnStateChanged(oldState, _currentState);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error changing state: {ex.Message}");
                // Optionally, revert to the previous state
                // currentState = stateHistory.Pop();
                throw;
            }
        }

        public void Update()
        {
            if (SynchronizationContext.Current == null)
            {
                UpdateAsync().GetAwaiter().GetResult();
            }
            else
            {
                UpdateAsync().Forget();
            }
        }

        public async UniTask UpdateAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var updateTasks = _states.Values.Select(state => state.UpdateAsync(cancellationToken));
                await UniTask.WhenAll(updateTasks);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during state update: {ex.Message}");
                throw;
            }
        }

        public void UndoLastTransition()
        {
            if (_stateHistory.Count > 0)
            {
                ChangeState(_stateHistory.Pop());
            }
            else
            {
                Debug.LogWarning("No previous state to revert to.");
            }
        }

        private void OnStateChanged(TState oldState, TState newState)
        {
            StateChanged?.Invoke(oldState, newState);
        }
    }
}