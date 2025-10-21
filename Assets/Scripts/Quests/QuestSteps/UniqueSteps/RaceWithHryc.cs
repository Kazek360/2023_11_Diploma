using Quests.QuestSteps.DialogueSteps;
using StorySystems.Dialogues;
using Tutorials;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class RaceWithHryc : AreaDialogueStep
    {
        [SerializeField] private TutorialInfo sprintTutorial;
        [SerializeField] private TutorialInfo inventoryTutorial;

        private bool _raceFinished;
        private bool _witFinished;
        private bool _hrycFinished;

        protected override void OnEnable()
        {
            base.OnEnable();
            _raceFinished = false;
            _witFinished = false;
            _hrycFinished = false;
        }

        private void Start()
        {
            TutorialManager.Instance.TriggerTutorial(sprintTutorial);
            PathEvents.StartMovement();
        }

        protected override void AreaEntered(string areaDialogueTrigger, TextAsset dialogue, string objectName)
        {
            if (triggerName.Equals(areaDialogueTrigger) && (objectName.Equals("Player") || objectName.Equals("Hryc")))
            {
                if (objectName.Equals("Player"))
                {
                    _witFinished = true;
                }
                else if (objectName.Equals("Hryc"))
                {
                    _hrycFinished = true;
                }

                if (!_raceFinished)
                {
                    if (objectName.Equals("Player"))
                    {
                        QuestManager.Instance.FlagManager.TriggerFlag("hryc_race_win");
                    }

                    _raceFinished = true;
                }

                if (_witFinished && _hrycFinished)
                {
                    DialogueEvent.InitializeDialogue();
                    DialogueEvent.StartDialogue(areaDialogueTrigger, dialogue);
                }
            }
        }
        protected override void FinishStep(string finishedDialogueTrigger)
        {
            if (triggerName.Equals(finishedDialogueTrigger))
            {
                TutorialManager.Instance.TriggerTutorial(inventoryTutorial);
                FinishAndDelete();
            } 
        }
    }
}