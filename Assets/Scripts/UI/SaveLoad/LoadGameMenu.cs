using InputSystem;
using SaveSystem;
using UI.SaveLoad;

namespace UI.Interfaces
{
    public class LoadGameMenu : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.LoadGame;
        private LoadSlot[] _loadSlots;
        
        protected override void Awake()
        {
            base.Awake();
            _loadSlots = GetComponentsInChildren<LoadSlot>();
        }
        
        public override void OnEnter()
        {
            var allGameData = DataPersistenceManager.Instance.LoadAllGameData();
            
            allGameData.TryGetValue("autosave", out var autosaveData);
            _loadSlots[0].SetData("autosave", autosaveData);
            
            for (var i = 1; i < _loadSlots.Length; i++)
            {
                var profile = $"save{i}";
                allGameData.TryGetValue(profile, out var profileData);
                _loadSlots[i].SetData(profile, profileData);
            }
            base.OnEnter();
            InputManager.Instance.Controlls.Player.Disable();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            InputManager.Instance.Controlls.Player.Enable();
        }
    }
}
