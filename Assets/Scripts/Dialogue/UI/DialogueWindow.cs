using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Class managing the currently triggered dialogue.
public class DialogueWindow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI sentenceText;
    
    [SerializeField] private List<GameObject> choices;
    private List<TextMeshProUGUI> _choicesText;
    
    private DialogueSO _currentDialogue;
    private SentenceSO _currentSentence;
    
    private void Start()
    {
        _choicesText = new List<TextMeshProUGUI>();
        for(int i = 0; i < choices.Count; i++)
        {
            contentParent.SetActive(false);
            _choicesText.Add(choices[i].GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    private void OnEnable()
    {
        DialogueEvents.OnDialogueStarted += StartDialogue;
    }
    
    private void OnDisable()
    {
        DialogueEvents.OnDialogueStarted -= StartDialogue;
    }
    
    private void StartDialogue(Dialogue dialogue)
    {
        contentParent.SetActive(true);
        InterfaceEvents.OpenInterface(gameObject.name);
        _currentDialogue = dialogue.Info;
        SetSentence(_currentDialogue.firstSentence);
    }
    
    public void Update()
    {
        if (contentParent.activeSelf && Input.GetButtonDown("Fire1") && _currentSentence.nextSentences.Count < 2)
        {
            if (_currentSentence.nextSentences.Count == 0)
            {
                if (_currentDialogue.questProgress != DialogueQuestProgress.None)
                {
                    if (_currentDialogue.questProgress == DialogueQuestProgress.Start)
                    {
                        QuestEvents.StartQuest(_currentDialogue.relatedQuest.id);
                    }
                    else if (_currentDialogue.questProgress == DialogueQuestProgress.Finish)
                    {
                        QuestEvents.FinishQuest(_currentDialogue.relatedQuest.id);
                    }
                }
                contentParent.SetActive(false);
                InterfaceEvents.CloseInterface();
                DialogueEvents.FinishDialogue(_currentDialogue);
            }
            else
            {
                SetSentence(_currentSentence.nextSentences[0]);
            }
        }
    }
    
    public void SetSentence(SentenceSO sentence)
    {
        _currentSentence = sentence;
        DisplayChoices();
        speakerPortrait.sprite = _currentSentence.portrait;
        speakerName.text= _currentSentence.speakerName;
        sentenceText.text = _currentSentence.text;

        if (_currentSentence.flagToTrigger is not null)
        {
            DialogueEvents.TriggerDialogueMarker(_currentSentence.flagToTrigger.id);
        }
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
