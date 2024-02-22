using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class DialogueWindowScript : MonoBehaviour
{
    private DialogueScriptableObject _currentDialogue;
    private SentenceScriptableObject _currentSentence;
    private int _currentSentenceNumber;
    
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [SerializeField] private Button buttonA;
    [SerializeField] private TextMeshProUGUI dialogueOptionA;
    [SerializeField] private Button buttonB;
    [SerializeField] private TextMeshProUGUI dialogueOptionB;
    [SerializeField] private GameObject dialogueOptionBox;
    
    public void StartDialogue(DialogueScriptableObject dialogueScriptableObject)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        _currentDialogue = dialogueScriptableObject;
        _currentSentenceNumber = 0;
        SetSentence(_currentSentenceNumber);
    }

    public void Update()
    {
        if (!Input.GetButtonDown("Fire1")) return;
        
        if (_currentSentence.IsChoice)
        {
            return;
        }
        
        _currentSentenceNumber++;
        
        if (_currentSentenceNumber >= _currentDialogue.GetDialogueLength())
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            return;
        }
        SetSentence(_currentSentenceNumber);
    }

    private void SetSentence(int i)
    {
        _currentSentence = _currentDialogue.GetSentence(i);
        foreach (string flag in _currentSentence.Flags)
        {
            SetFlag(flag);
        }
        if (_currentSentence.IsChoice)
        {
            dialogueOptionBox.SetActive(true);
            dialogueOptionA.text = _currentSentence.ChoiceA;
            dialogueOptionB.text = _currentSentence.ChoiceB;
            buttonA.onClick.AddListener(() => StartDialogue(_currentSentence.DialogueA));
            buttonB.onClick.AddListener(() => StartDialogue(_currentSentence.DialogueB));
        }
        else
        {
            dialogueOptionBox.SetActive(false);
        }

        speakerPortrait.sprite = _currentSentence.SpeakerPortrait;
        speakerName.text= _currentSentence.SpeakerName;
        sentenceText.text = _currentSentence.Text;
    }


    private void SetFlag(string str)
    {
        //TODO: Make flags settable
        Debug.Log("FLAG SET: " + str);
        
    }
}
