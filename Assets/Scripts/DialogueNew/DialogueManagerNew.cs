using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Class managing the currently triggered dialogue.
public class DialogueManagerNew : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI sentenceText;
    
    [SerializeField] private List<GameObject> choices;
    private List<TextMeshProUGUI> _choicesText;
    
    private DialogueSONew _currentDialogue;
    private SentenceSONew _currentSentence;

    private void Start()
    {
        _choicesText = new List<TextMeshProUGUI>();
        for (int i = 0; i < choices.Count; i++)
        {
            dialogueWindow.SetActive(false);
            _choicesText.Add(choices[i].GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    private void OnEnable()
    {
        DialogueEventsNew.OnDialogueStarted += StartDialogue;
    }
    
    private void OnDisable()
    {
        DialogueEventsNew.OnDialogueStarted -= StartDialogue;
    }
    
    private void StartDialogue(DialogueNew dialogue)
    {
        dialogueWindow.SetActive(true);
        _currentDialogue = dialogue.Info;
        SetSentence(_currentDialogue.firstSentence);
    }
    
    public void Update()
    {
        if (dialogueWindow.activeSelf && Input.GetButtonDown("Fire1") && _currentSentence.nextSentences.Count < 2)
        {
            if (_currentSentence.nextSentences.Count == 0)
            {
                dialogueWindow.SetActive(false);
                DialogueEvents.EndDialogue();
            }
            else
            {
                SetSentence(_currentSentence.nextSentences[0]);
            }
        }
    }
    
    public void SetSentence(SentenceSONew sentence)
    {
        _currentSentence = sentence;
        DisplayChoices();
        speakerPortrait.sprite = _currentSentence.portrait;
        speakerName.text= _currentSentence.speakerName;
        sentenceText.text = _currentSentence.text;
    }

    private void DisplayChoices()
    {
        int index = 0;
        foreach (var answer in _currentSentence.answers)
        {
            choices[index].SetActive(true);
            _choicesText[index].text = answer;
            index++;
        }

        for (int i = index; i < choices.Count; i++)
        {
            choices[i].SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        SetSentence(_currentSentence.nextSentences[choiceIndex]);
    }
}
