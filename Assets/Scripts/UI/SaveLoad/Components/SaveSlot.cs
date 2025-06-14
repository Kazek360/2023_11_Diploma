using Location;
using SaveSystem;
using SaveSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SaveLoad
{
    [RequireComponent(typeof(Button))]
    public class SaveSlot : MonoBehaviour
    {
        private string _profileName;
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
                HideData();
            }
            else
            {
                levelText.text = $"Poziom: {gameData.resources.level}";
                locationText.text = gameData.scene.displayedName;
                dateText.text = gameData.save.saveDate;
                gameTimeText.text = $"Czas Gry: {gameData.save.gameTime}";
                ShowData();
            }

            _profileName = profile;
            _saveSlotButton.onClick.AddListener(Save);
        }
        
        private void Save()
        { 
            DataPersistenceManager.Instance.SaveGame(_profileName);
            levelText.text = $"Poziom: {PlayerStats.GetInstance().Level}";
            locationText.text = SceneInfo.Instance.displayedSceneName;
            dateText.text = DataPersistenceManager.Instance.CurrentGameData.save.saveDate;
            gameTimeText.text = $"Czas Gry: {DataPersistenceManager.Instance.CurrentGameData.save.gameTime}";
            ShowData();
        }

        private void HideData()
        {
            noSaveText.alpha = 255;
            levelText.alpha = 0;
            locationText.alpha = 0;
            dateText.alpha = 0;
            gameTimeText.alpha = 0;
        }
        
        private void ShowData()
        {
            noSaveText.alpha = 0;
            levelText.alpha = 255;
            locationText.alpha = 255;
            dateText.alpha = 255;
            gameTimeText.alpha = 255;
        }
    }
}


