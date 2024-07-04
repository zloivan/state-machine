using UnityEngine;

namespace Utilities.StateMachine.Examples
{
    public class AppStateMachine : MonoBehaviour
    {
        private StateMachine<AppState> _stateMachine;

        private void Start()
        {
            _stateMachine = new StateMachine<AppState>();

            _stateMachine.AddState<BootstrapState>(AppState.Bootstrap);
            _stateMachine.AddState<MainMenuState>(AppState.MainMenu);
            _stateMachine.AddState<GameLoopState>(AppState.GameLoop);

            _stateMachine.AddTransition(AppState.Bootstrap, AppState.MainMenu,
                () => Time.realtimeSinceStartup > 10f); // EXAMPLE OF GUARD, HERE CAN BE ANITHING
            _stateMachine.AddTransition(AppState.GameLoop, AppState.MainMenu, () => true);

            _stateMachine.StateChanged += (oldState, newState) =>
                Debug.Log($"State changed from {oldState} to {newState}");

            _stateMachine.SetInitialState(AppState.Bootstrap);
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}