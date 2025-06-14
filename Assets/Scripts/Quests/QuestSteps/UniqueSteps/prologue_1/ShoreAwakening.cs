using System.Collections;
using Other;
using Quests.QuestSteps.DialogueSteps;
using StorySystems.Dialogues;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class ShoreAwakening : DialogueStep
    {
        [SerializeField] protected string areaToEnterName;

        private GameObject _area;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            AreaEvent.OnAreaEnter += StartDialogue;
            _area = GameObject.Find(areaToEnterName);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AreaEvent.OnAreaEnter -= StartDialogue;
        }

        protected override IEnumerator Start()
        {
            yield return base.Start();
            Nestor.ChangeNestorState("shore", true);
        }
        
        private void StartDialogue(string areaName, string objectName)
        {
            if (areaName.Equals(areaToEnterName))
            {
                Destroy(_area);
                DialogueManager.Instance.TriggerDialogue("ShorePrologue");
            }
        }

        protected override void FinishStep(DialogueInfo dialogue)
        {
            if (dialogueInfo.name == dialogue.name)
            {
                FinishAndDelete();
            } 
        }
    }
}