using System;
using System.Collections.Generic;
using Shared;
using UI.Interfaces;
using UnityEngine;

namespace UI
{
    public class UserInterfaceManager : Singleton<UserInterfaceManager>
    {
        private Dictionary<GameState, UserInterface> _states;
        public Dictionary<GameState, UserInterface> States => _states;

        [SerializeField] private GameState initialState;
        private GameState _previousState;
        public GameState PreviousState => _previousState;
        
        private UserInterface _currentState;
        public GameState CurrentStateType => _currentState.State;
        
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
        public void ChangeState(GameState state)
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
    public enum GameState { MainMenu, PauseMenu, LoadGame, SaveGame, Settings, Exploration, Quests, Dialogue, Cutscene, 
        SkillTree, Abilities }
}
