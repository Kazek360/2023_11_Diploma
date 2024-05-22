using System;
using SaveSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")] 
    [SerializeField] public string profileId;

    [Header("Components")] 
    [SerializeField] private GameObject noSaveData;
    [SerializeField] private GameObject hasSaveData;
    [SerializeField] private TextMeshProUGUI saveSlotText;

    private Button _saveSlotButton;

    private void Awake()
    {
        _saveSlotButton = GetComponent<Button>();
    }
    
    public void SetData(GameData gameData)
    {
        if (gameData is null)
        {
            noSaveData.SetActive(true);
            hasSaveData.SetActive(false);
        }
        else
        {
            noSaveData.SetActive(false);
            hasSaveData.SetActive(true);

            saveSlotText.text = $"{gameData.currentScene} - {gameData.saveDate}";
        }
    }

    public void SetInteractable(bool interactable)
    {
        _saveSlotButton.interactable = interactable;
    }

}
