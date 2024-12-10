using System.Collections.Generic;
using Quests.UI;
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

        private void OnEnable()
        {
            if (States is null)
            {
                _states = new();
                foreach (var state in FindObjectsOfType<UserInterface>())
                    _states.Add(state.State, state);
            }
            ChangeState(initialState);
            QuestPopup.isInterfaceAssigned = true;
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
                if (_currentState.State is UserInterfaceState.Exploration or UserInterfaceState.Combat)
                {
                    _previousState = _currentState.State;
                }
                _currentState.OnExit();
            }
            _currentState = States[state];
            _currentState.OnEnter();
        }
        
        /// <summary>
        /// Changes UI state and calls OnEntered
        /// </summary>
        /// <param name="state"></param>
        public void ChangeStateWithoutVisibilityChange(UserInterfaceState state)
        {
            if (!States.ContainsKey(state))
            {
                Debug.LogError("Tried to change UI state to non-existing one!");
                return;
            }
            if (_currentState is not null){
                if (_currentState.State is UserInterfaceState.Tutorial)
                {
                    _previousState = _currentState.State;
                }
            }
            _currentState = States[state];
            _currentState.OnEnter();
        }
        
    }
    public enum UserInterfaceState { MainMenu, PauseMenu, LoadGame, SaveGame, Settings, Exploration, Quests, Dialogue, Cutscene, 
        SkillTree, Abilities, Inventory, Combat, LoadingScreen, Map, Tutorial, Credits, GameStart }
}
