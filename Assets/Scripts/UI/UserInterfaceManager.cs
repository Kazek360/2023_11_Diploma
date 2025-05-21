using System;
using System.Collections.Generic;
using Quests.UI;
using Shared;
using UI.Interfaces;
using UnityEngine;

namespace UI
{
    public class UserInterfaceManager : Singleton<UserInterfaceManager>
    {
        [SerializeField] private UserInterfaceState initialState;
        private Dictionary<UserInterfaceState, UserInterface> _states = new();
        private UserInterfaceState _previousState;
        public UserInterfaceState PreviousState => _previousState;
        private UserInterface _currentState;
        public UserInterfaceState CurrentStateType => _currentState.State;
        public static bool isInitialized;
        
        public static event Action onInterfacePinged;
        public static void PingInterface()
        {
            onInterfacePinged?.Invoke();
        }
        public static event Action<UserInterfaceState, UserInterface> onInterfaceSubscribed;
        public static void SubscribeToManager(UserInterfaceState state, UserInterface userInterface)
        {
            onInterfaceSubscribed?.Invoke(state, userInterface);
        }

        protected override void Awake()
        {
            base.Awake();
            isInitialized = false;
        }

        private void OnEnable()
        {
            onInterfaceSubscribed += AddToDictionary;
        }
        
        private void OnDisable()
        {
            onInterfaceSubscribed -= AddToDictionary;
        }

        private void Start()
        {
            PingInterface();
            isInitialized = true;
        }

        /// <summary>
        /// Changes UI state and calls OnExit and OnEntered
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(UserInterfaceState state)
        {
            if (!_states.ContainsKey(state))
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
            _currentState = _states[state];
            _currentState.OnEnter();
        }
        
        /// <summary>
        /// Changes UI state and calls OnEntered
        /// </summary>
        /// <param name="state"></param>
        public void ChangeStateWithoutVisibilityChange(UserInterfaceState state)
        {
            if (!_states.ContainsKey(state))
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
            _currentState = _states[state];
            _currentState.OnEnter();
        }

        private void AddToDictionary(UserInterfaceState state, UserInterface userInterface)
        {
            _states.Add(state, userInterface);
            if (initialState == state)
            {
                ChangeState(initialState);
            }
        }
    }
    public enum UserInterfaceState { MainMenu, PauseMenu, LoadGame, SaveGame, Settings, Exploration, Quests, Dialogue, Cutscene, 
        SkillTree, Abilities, Inventory, Combat, LoadingScreen, Map, Tutorial, Credits, GameStart, ChapterIntroduction }
}
