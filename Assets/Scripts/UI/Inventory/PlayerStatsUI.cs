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
        HealthText.text = "Życie: " + PlayerStats.GetInstance().GetStat(StatType.WitHealth);
        AgilityText.text = "Zręczność: " + PlayerStats.GetInstance().GetStat(StatType.WitAgility);
        StrengthText.text = "Siła: " + PlayerStats.GetInstance().GetStat(StatType.WitStrength);
        LuckText.text = "Szczęście: " + PlayerStats.GetInstance().GetStat(StatType.WitLuck);
        ImaginationText.text = "Wyobraźnia: " + PlayerStats.GetInstance().GetStat(StatType.WitImagination);
    }
}
