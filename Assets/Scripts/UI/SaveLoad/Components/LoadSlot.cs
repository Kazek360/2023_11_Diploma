using Location;
using SaveSystem;
using SaveSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SaveLoad
{
    [RequireComponent(typeof(Button))]
    public class LoadSlot : MonoBehaviour
    {
        private GameData _profileData;
        private Button _saveSlotButton;
        [SerializeField] private TextMeshProUGUI noSaveText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI locationText;
        [SerializeField] private TextMeshProUGUI dateText;
        [SerializeField] private TextMeshProUGUI gameTimeText;
    
        private void Awake()
        {
            _saveSlotButton = GetComponent<Button>();
        }
        
        public void SetData(string profile, GameData gameData)
        {
            _saveSlotButton.onClick.RemoveAllListeners();
            
            if (gameData is null)
            {
                noSaveText.alpha = 255;
                noSaveText.text = "Brak Zapisu";
                levelText.alpha = 0;
                locationText.alpha = 0;
                dateText.alpha = 0;
                gameTimeText.alpha = 0;
                _saveSlotButton.interactable = false;
            }
            else
            {
                _saveSlotButton.interactable = true;
                levelText.alpha = 255;
                levelText.text = $"Poziom: {gameData.resources.level}";
                locationText.alpha = 255;
                locationText.text = gameData.scene.displayedName;
                dateText.alpha = 255;
                dateText.text = gameData.save.saveDate;
                gameTimeText.alpha = 255;
                gameTimeText.text = $"Czas Gry: {gameData.save.gameTime}";
                
                if (profile.Equals("autosave"))
                {
                    noSaveText.text = "Automatyczny Zapis";
                }
                else
                {
                    noSaveText.alpha = 0;
                }
                _profileData = gameData;
                _saveSlotButton.onClick.AddListener(Load);
            }
        }
        
        private void Load()
        {
            DataPersistenceManager.Instance.CurrentLoadMode = LoadMode.FromSaveFile;
            DataPersistenceManager.Instance.ChangeCurrentGameData(_profileData);
            SceneLoader.InvokeOnSceneChanged(DataPersistenceManager.Instance.CurrentGameData.scene.name);
        }
    }
}