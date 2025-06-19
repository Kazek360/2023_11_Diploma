namespace Quests.QuestSteps.OtherSteps
{
    public class HealStep : BaseQuestStep
    {
        private void Start()
        {
            PlayerStats.GetInstance().WitHp = 8;
            PlayerStats.GetInstance().NestorHp = 8;
            FinishAndDelete();
        }
    }
}