namespace Quests.QuestSteps.OtherSteps
{
    public class HealStep : BaseQuestStep
    {
        private void Start()
        {
            PlayerStats.GetInstance().WitHp = PlayerStats.GetInstance().GetStat(StatType.WitHealth);
            PlayerStats.GetInstance().NestorHp = PlayerStats.GetInstance().GetStat(StatType.NestorHealth);;
            FinishAndDelete();
        }
    }
}