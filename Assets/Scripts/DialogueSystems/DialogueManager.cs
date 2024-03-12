using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI sentenceText;
    
    private DialogueSO _currentDialogue;
    private SentenceSO _currentSentence;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueStarted += StartDialogue;
    }
    
    private void OnDisable()
    {
        DialogueEvents.OnDialogueStarted -= StartDialogue;
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueWindow.SetActive(true);
        Time.timeScale = 0f;
        _currentDialogue = dialogue.Info;
        SetSentence(_currentDialogue.firstSentence);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_currentSentence.nextSentence is null)
            {
                Time.timeScale = 1f;
                dialogueWindow.SetActive(false);
            }
            else
            {
                SetSentence(_currentSentence.nextSentence);
            }
        }
    }

    private void SetSentence(SentenceSO sentence)
    {
        _currentSentence = sentence;
        
        speakerPortrait.sprite = _currentSentence.portrait;
        speakerName.text= _currentSentence.speakerName;
        sentenceText.text = _currentSentence.text;
    }
}
