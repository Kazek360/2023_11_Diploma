using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI agilityText;
    [SerializeField] private TextMeshProUGUI luckText;
    [SerializeField] private TextMeshProUGUI imaginationText;
    
    public void UpdateDisplay()
    {
        levelText.text = $"Poziom: {PlayerStats.GetInstance().Level}";
        experienceText.text = $"XP: {PlayerStats.GetInstance().CurrentXp}/1000";
        healthText.text = $"Życie: {PlayerStats.GetInstance().WitHp}/{PlayerStats.GetInstance().GetStat(StatType.WitHealth)}";
        strengthText.text = $"Siła: {PlayerStats.GetInstance().GetStat(StatType.WitStrength)}";
        agilityText.text = $"Zręczność: {PlayerStats.GetInstance().GetStat(StatType.WitAgility)}";
        luckText.text = $"Szczęście: {PlayerStats.GetInstance().GetStat(StatType.WitLuck)}";
        imaginationText.text = $"Wyobraźnia: {PlayerStats.GetInstance().FreeImagination}/{PlayerStats.GetInstance().GetStat(StatType.WitImagination)}";
    }
}
