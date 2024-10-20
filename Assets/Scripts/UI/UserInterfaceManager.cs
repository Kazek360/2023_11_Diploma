using System.Collections.Generic;
using Shared;
using UI.Interfaces;
using UnityEngine;

namespace UI
{
    public class UserInterfaceManager : Singleton<UserInterfaceManager>
    {
        private Dictionary<UserInterfaceState, UserInterface> _states;
        public Dictionary<UserInterfaceState, UserInterface> States => _states;

        [SerializeField] private UserInterfaceState initialState;
        private UserInterfaceState _previousState;
        public UserInterfaceState PreviousState => _previousState;
        
        private UserInterface _currentState;
        public UserInterfaceState CurrentStateType => _currentState.State;
        
        private new void Awake()
        {
            base.Awake();
            if (States is null)
            {
                _states = new();
                foreach (var state in FindObjectsOfType<UserInterface>())
                    _states.Add(state.State, state);
            }
        }

        private void Start()
        {
            ChangeState(initialState);
        }

        /// <summary>
        /// Changes UI state and calls OnExit and OnEntered
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(UserInterfaceState state)
        {
            if (!States.ContainsKey(state))
            {
                Debug.LogError("Tried to change UI state to non-existing one!");
                return;
            }
            if (_currentState is not null){
                _previousState = _currentState.State;
                _currentState.OnExit();
            }
            _currentState = States[state];
            _currentState.OnEnter();
        }
    }
    public enum UserInterfaceState { MainMenu, PauseMenu, LoadGame, SaveGame, Settings, Exploration, Quests, Dialogue, Cutscene, 
        SkillTree, Abilities, Inventory, Combat }
}
