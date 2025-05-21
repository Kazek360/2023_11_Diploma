using System.Collections;
using Dialogues;
using Other;
using Quests.QuestSteps.DialogueSteps;
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
            AreaEvent.OnAreaEnter += DisappearNestor;
            _area = GameObject.Find(areaToEnterName);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AreaEvent.OnAreaEnter -= DisappearNestor;
        }

        protected override IEnumerator Start()
        {
            yield return base.Start();
            Nestor.ChangeNestorState("shore", true);
        }
        
        private void DisappearNestor(string areaName, string objectName)
        {
            if (areaName.Equals(areaToEnterName))
            {
                Destroy(_area);
                Nestor.ChangeNestorState("shore", false);
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