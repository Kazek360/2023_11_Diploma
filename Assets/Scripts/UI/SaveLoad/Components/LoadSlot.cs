using Location;
using SaveSystem;
using SaveSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.SaveLoad
{
    [RequireComponent(typeof(Button))]
    public class LoadSlot : MonoBehaviour
    {
        private GameData _profileData;
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
                _saveSlotButton.interactable = false;
            }
            else
            {
                _saveSlotButton.interactable = true;
                if (profile.Equals("autosave"))
                {
                    _saveSlotText.fontSize = 30;
                    _saveSlotText.text = $"(automatyczny zapis)\n{gameData.scene.displayedName} - {gameData.saveDate}"; 
                }
                else
                {
                    _saveSlotText.text = $"{gameData.scene.displayedName} - {gameData.saveDate}"; 
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