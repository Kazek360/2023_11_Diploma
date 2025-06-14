using System.Collections;
using Quests.QuestSteps.DialogueSteps;
using StorySystems.Dialogues;
using Tutorials;
using UI;
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

        private new IEnumerator Start()
        {
            while (!UserInterfaceManager.isInitialized)
            {
                yield return null;
            }

            TutorialManager.Instance.TriggerTutorial(sprintTutorial);
            PathEvents.StartMovement();
        }

        protected override void AreaEntered(string areaName, string objectName)
        {
            if (areaName.Equals(areaToEnterName) && (objectName.Equals("Player") || objectName.Equals("Hryc")))
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
                    AreaEvent.EnterCorrectArea(areaToEnterName);
                }
            }
        }

        protected override void FinishStep(DialogueInfo dialogue)
        {
            if (dialogueInfo.name == dialogue.name)
            {
                TutorialManager.Instance.TriggerTutorial(inventoryTutorial);
                FinishAndDelete();
            }
        }
    }
}