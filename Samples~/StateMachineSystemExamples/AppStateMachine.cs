using IKhom.StateMachineSystem.Runtime;
using UnityEngine;

namespace IKhom.StateMachineSystem.Samples.StateMachineSystemExamples
{
    public class AppStateMachine : MonoBehaviour
    {
        private StateMachine<AppState> _stateMachine;

        private void Start()
        {
            _stateMachine = new StateMachine<AppState>();

            _stateMachine.AddState<MainMenuState>(AppState.MainMenu); // Easy state adding with parameterless constructor
            
            // sending context to state to be able to change state from state itself
            _stateMachine.AddStateWithContext(AppState.Bootstrap, context => new BootstrapState(context)); 
            _stateMachine.AddStateWithContext(AppState.GameLoop, _ => new GameLoopState());

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