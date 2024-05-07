using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI AgilityText;
    [SerializeField] private TextMeshProUGUI StrengthText;
    [SerializeField] private TextMeshProUGUI LuckText;
    [SerializeField] private TextMeshProUGUI ImaginationText;


    public void UpdateDisplay()
    {
        HealthText.text = "Życie: " + GameConfig.PlayerStats.GetStat(StatType.Health);
        AgilityText.text = "Zręczność: " + GameConfig.PlayerStats.GetStat(StatType.Agility);
        StrengthText.text = "Siła: " + GameConfig.PlayerStats.GetStat(StatType.Strength);
        LuckText.text = "Szczęście: " + GameConfig.PlayerStats.GetStat(StatType.Luck);
        ImaginationText.text = "Wyobraźnia: " + GameConfig.PlayerStats.GetStat(StatType.Imagination);
    }
}
