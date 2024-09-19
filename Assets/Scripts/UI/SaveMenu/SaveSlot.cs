using System;
using SaveSystem;
using SaveSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class SaveSlot : MonoBehaviour
{
    private GameData _profileData;
    private Button _saveSlotButton;
    private TextMeshProUGUI _saveSlotText;

    private void Awake()
    {
        _saveSlotButton = GetComponent<Button>();
        _saveSlotText = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void SetData(string buttonMode, string profile, GameData gameData)
    {
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
                _saveSlotText.text = $"(automatyczny zapis)\n{gameData.currentScene} - {gameData.saveDate}"; 
            }
            else
            {
                _saveSlotText.text = $"{gameData.currentScene} - {gameData.saveDate}"; 
            }
            
            _profileData = gameData;

            if (buttonMode.Equals("load"))
            {
                _saveSlotButton.onClick.AddListener(LoadAction);
            }
            else if (buttonMode.Equals("save"))
            {
                
            }
        }
    }
    
    private void LoadAction()
    {
        DataPersistenceManager.Instance.CurrentLoadMode = LoadMode.FromSaveFile;
        DataPersistenceManager.Instance.ChangeCurrentGameData(_profileData);
        SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.CurrentGameData.currentScene);
    }
}
