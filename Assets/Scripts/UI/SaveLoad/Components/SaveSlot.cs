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
        private TextMeshProUGUI _saveSlotText;
    
        private void Awake()
        {
            _saveSlotButton = GetComponent<Button>();
            _saveSlotText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        public void SetData(string profile, GameData gameData)
        {
            _saveSlotButton.onClick.RemoveAllListeners();
            
            if (gameData is null)
            {
                _saveSlotText.text = "Brak Zapisu";
            }
            else
            {
                _saveSlotText.text = $"{gameData.currentScene} - {gameData.saveDate}"; 
            }

            _profileName = profile;
            _saveSlotButton.onClick.AddListener(Save);
        }
        
        private void Save()
        { 
            DataPersistenceManager.Instance.SaveGame(_profileName);
            _saveSlotText.text = $"{DataPersistenceManager.Instance.CurrentGameData.currentScene} - " +
                                 $"{DataPersistenceManager.Instance.CurrentGameData.saveDate}"; 
        }
    }
}


