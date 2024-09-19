using SaveSystem;

namespace UI.Interfaces
{
    public class LoadGameMenu : UserInterface
    {
        public override GameState State => GameState.LoadGame;
        private SaveSlot[] _saveSlots;
        
        private new void Awake()
        {
            base.Awake();
            _saveSlots = GetComponentsInChildren<SaveSlot>();
        }
        
        public override void OnEnter()
        {
            var allGameData = DataPersistenceManager.Instance.LoadAllGameData();
            
            allGameData.TryGetValue("autosave", out var autosaveData);
            _saveSlots[0].SetData("load", "autosave", autosaveData);
            
            for (var i = 1; i < _saveSlots.Length; i++)
            {
                var profile = $"save{i}";
                allGameData.TryGetValue(profile, out var profileData);
                _saveSlots[i].SetData("load", profile, profileData);
            }
            base.OnEnter();
        }
    }
}
