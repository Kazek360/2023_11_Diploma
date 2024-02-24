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
    private int _currentSentenceNumber;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueStart += StartDialogue;
    }
    
    private void OnDisable()
    {
        DialogueEvents.OnDialogueStart -= StartDialogue;
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueWindow.SetActive(true);
        Time.timeScale = 0f;
        _currentDialogue = dialogue.Info;
        _currentSentenceNumber = 0;
        SetSentence(_currentSentenceNumber);
    }

    public void Update()
    {
        if (!Input.GetButtonDown("Fire1")) return;
        
        _currentSentenceNumber++;
        
        if (_currentSentenceNumber >= _currentDialogue.sentences.Count)
        {
            Time.timeScale = 1f;
            dialogueWindow.SetActive(false);
            return;
        }
        SetSentence(_currentSentenceNumber);
    }

    private void SetSentence(int i)
    {
        _currentSentence = _currentDialogue.sentences[i];
        
        speakerPortrait.sprite = _currentSentence.Portrait;
        speakerName.text= _currentSentence.SpeakerName;
        sentenceText.text = _currentSentence.Text;
    }
}
