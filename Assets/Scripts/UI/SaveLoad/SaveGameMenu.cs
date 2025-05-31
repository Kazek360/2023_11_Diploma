using InputSystem;
using SaveSystem;
using UI.Interfaces;

namespace UI.SaveLoad
{
    public class SaveGameMenu  : UserInterface
    {
        public override UserInterfaceState State => UserInterfaceState.SaveGame;
        private SaveSlot[] _saveSlots;
        
        protected override void Awake()
        {
            base.Awake();
            _saveSlots = GetComponentsInChildren<SaveSlot>();
        }
        
        public override void OnEnter()
        {
            var allGameData = DataPersistenceManager.Instance.LoadAllGameData();
            
            for (var i = 0; i < _saveSlots.Length; i++)
            {
                var profile = $"save{i + 1}";
                allGameData.TryGetValue(profile, out var profileData);
                _saveSlots[i].SetData(profile, profileData);
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