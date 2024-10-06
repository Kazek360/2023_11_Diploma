using SaveSystem;
using UI.SaveLoad;

namespace UI.Interfaces
{
    public class SaveGameMenu  : UserInterface
    {
        public override GameState State => GameState.SaveGame;
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
        }
    }
}