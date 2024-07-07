using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Utilities.StateMachine.abstractions;
using Utilities.StateMachine.helpers;

namespace Utilities.StateMachine
{
    /// <summary>
    /// Represents a generic state machine.
    /// </summary>
    /// <typeparam name="TState">The type of the state enum.</typeparam>
    public class StateMachine<TState> : IStateContext<TState> where TState : Enum
    {
        private TState _currentState;
        private readonly Dictionary<TState, IState<TState>> _states = new();
        private readonly Stack<TState> _stateHistory = new();
        private readonly Dictionary<(TState, TState), Func<bool>> _transitionGuards = new();
        private readonly StateMachineLogger _logger = new();
        private readonly Dictionary<TState, UniTask> _cachedUpdateTasks = new();
        private readonly object _lock = new();

        /// <summary>
        /// Gets the current state of the state machine.
        /// </summary>
        public TState CurrentState => _currentState;

        /// <summary>
        /// Event triggered when the state changes.
        /// </summary>
        public event Action<TState, TState> StateChanged;

        /// <summary>
        /// Adds a new state to the state machine.
        /// </summary>
        /// <typeparam name="TStateImpl">The type of the state implementation.</typeparam>
        /// <param name="stateKey">The enum key for the state.</param>
        public void AddState<TStateImpl>(TState stateKey) where TStateImpl : IState<TState>, new()
        {
            AddState(stateKey, () => new TStateImpl());
        }

        /// <summary>
        /// Adds a new state to the state machine with a context-aware factory method.
        /// </summary>
        /// <typeparam name="TStateImpl">The type of the state implementation.</typeparam>
        /// <param name="stateKey">The enum key for the state.</param>
        /// <param name="stateFactory">A factory method to create the state instance.</param>
        public void AddStateWithContext<TStateImpl>(TState stateKey, Func<IStateContext<TState>, TStateImpl> stateFactory) where TStateImpl : IState<TState>
        {
            lock (_lock)
            {
                if (_states.ContainsKey(stateKey))
                {
                    _logger.LogWarning($"State {stateKey} already exists in the state machine. Overwriting.");
                }
                _states[stateKey] = stateFactory(this);
                _cachedUpdateTasks[stateKey] = UniTask.CompletedTask;
            }
        }

        /// <summary>
        /// Adds a transition guard between two states.
        /// </summary>
        /// <param name="from">The source state.</param>
        /// <param name="to">The destination state.</param>
        /// <param name="guard">A function that returns true if the transition is allowed.</param>
        public void AddTransition(TState from, TState to, Func<bool> guard)
        {
            lock (_lock)
            {
                _transitionGuards[(from, to)] = guard;
            }
        }

        /// <summary>
        /// Sets the initial state of the state machine.
        /// </summary>
        /// <param name="initialState">The initial state to set.</param>
        public void SetInitialState(TState initialState)
        {
            AsyncUtility.RunSynchronous(() => SetInitialStateAsync(initialState));
        }

        /// <summary>
        /// Sets the initial state of the state machine asynchronously.
        /// </summary>
        /// <param name="initialState">The initial state to set.</param>
        public async UniTask SetInitialStateAsync(TState initialState)
        {
            lock (_lock)
            {
                if (!_states.ContainsKey(initialState))
                {
                    throw new StateMachineException($"State {initialState} does not exist in the state machine.");
                }
            }

            _currentState = initialState;
            _stateHistory.Clear();
            await _states[_currentState].EnterAsync();
            OnStateChanged(default, _currentState);
        }

        /// <summary>
        /// Changes the current state of the state machine.
        /// </summary>
        /// <param name="newState">The new state to transition to.</param>
        public void ChangeState(TState newState)
        {
            AsyncUtility.RunSynchronous(() => ChangeStateAsync(newState));
        }

        /// <summary>
        /// Changes the current state of the state machine asynchronously.
        /// </summary>
        /// <param name="newState">The new state to transition to.</param>
        public async UniTask ChangeStateAsync(TState newState)
        {
            lock (_lock)
            {
                if (!_states.ContainsKey(newState))
                {
                    throw new StateMachineException($"State {newState} does not exist in the state machine.");
                }

                if (_transitionGuards.TryGetValue((_currentState, newState), out var guard) && !guard())
                {
                    _logger.LogWarning($"Transition from {_currentState} to {newState} is not allowed by the guard.");
                    return;
                }
            }

            var oldState = _currentState;
            await _states[_currentState].ExitAsync();
            _stateHistory.Push(_currentState);
            _currentState = newState;
            await _states[_currentState].EnterAsync();
            OnStateChanged(oldState, _currentState);
        }

        /// <summary>
        /// Updates the current state.
        /// </summary>
        public void Update()
        {
            AsyncUtility.RunSynchronous(UpdateAsync);
        }

        /// <summary>
        /// Updates the current state asynchronously.
        /// </summary>
        public async UniTask UpdateAsync()
        {
            IState<TState> currentState;
            lock (_lock)
            {
                currentState = _states[_currentState];
            }

            _cachedUpdateTasks[_currentState] = currentState.UpdateAsync();
            await _cachedUpdateTasks[_currentState];
        }

        /// <summary>
        /// Reverts to the previous state.
        /// </summary>
        public void UndoLastTransition()
        {
            lock (_lock)
            {
                if (_stateHistory.Count > 0)
                {
                    ChangeState(_stateHistory.Pop());
                }
                else
                {
                    _logger.LogWarning("No previous state to revert to.");
                }
            }
        }

        private void AddState(TState stateKey, Func<IState<TState>> stateFactory)
        {
            lock (_lock)
            {
                if (_states.ContainsKey(stateKey))
                {
                    _logger.LogWarning($"State {stateKey} already exists in the state machine. Overwriting.");
                }
                _states[stateKey] = stateFactory();
                _cachedUpdateTasks[stateKey] = UniTask.CompletedTask;
            }
        }

        private void OnStateChanged(TState oldState, TState newState)
        {
            _logger.Log($"State changed from {oldState} to {newState}");
            StateChanged?.Invoke(oldState, newState);
        }
    }
}