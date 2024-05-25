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
        HealthText.text = "Życie: " + GameConfig.PlayerStats.GetStat(StatType.WitHealth);
        AgilityText.text = "Zręczność: " + GameConfig.PlayerStats.GetStat(StatType.WitAgility);
        StrengthText.text = "Siła: " + GameConfig.PlayerStats.GetStat(StatType.WitStrength);
        LuckText.text = "Szczęście: " + GameConfig.PlayerStats.GetStat(StatType.WitLuck);
        ImaginationText.text = "Wyobraźnia: " + GameConfig.PlayerStats.GetStat(StatType.WitImagination);
    }
}
