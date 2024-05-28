using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotMenu : MonoBehaviour
{
    [SerializeField] private GameObject backMenu;
    [SerializeField] private TextMeshProUGUI screenName;
    private SaveSlot[] _saveSlots;

    private string _currentOperation;
    private Dictionary<string, GameData> _profilesGameData;
    
    private void Awake()
    {
        _saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void ActivateMenu(string operation)
    {
        _currentOperation = operation;
        gameObject.SetActive(true);
        
        _profilesGameData = DataPersistenceManager.Instance.GetAllGameData();
        foreach (var saveSlot in _saveSlots)
        {
            _profilesGameData.TryGetValue(saveSlot.profileId, out var profileData);
            saveSlot.SetData(profileData);

            if (_currentOperation.Equals("load"))
            {
                if (screenName is not null)
                {
                    screenName.text = "Wczytaj grę";
                }
                saveSlot.SetInteractable(profileData != null);
            }
            else if (_currentOperation.Equals("save"))
            {
                if (screenName is not null)
                {
                    screenName.text = "Zapisz grę";
                }
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        
        if (_currentOperation.Equals("load"))
        {
            DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.profileId);
            DataPersistenceManager.Instance.LoadGameData();
            SceneManager.LoadSceneAsync(DataPersistenceManager.Instance.GameData.currentScene);
        }
        else if (_currentOperation.Equals("save"))
        {
            DataPersistenceManager.Instance.SaveGame(saveSlot.profileId);
            DeactivateMenu();
        }
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
        backMenu.SetActive(true);
    }
}
