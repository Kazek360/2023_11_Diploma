using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSummaryScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ResultText;
    [SerializeField] private GameObject PlayerScrollContent;
    [SerializeField] private GameObject EnemyScrollContent;
    [SerializeField] private GameObject EntityContainerPrefab;
    [SerializeField] private GameObject ProceedWin;
    [SerializeField] private GameObject ProceedLoss;

    public void SetValues(bool IsVictory, List<IHittable> players, List<IHittable> enemies)
    {
        if (IsVictory)
        {
            ProceedWin.SetActive(true);
            ProceedLoss.SetActive(false);
            ResultText.text = "ZWYCIĘSTWO";
        }
        else
        {
            ProceedWin.SetActive(false);
            ProceedLoss.SetActive(true);
            ResultText.text = "PORAŻKA";
        }

        foreach (IHittable player in players)
        {
            var container = Instantiate(EntityContainerPrefab, PlayerScrollContent.transform);
            container.GetComponent<EntityDisplaySummaryScript>().SetValues(player);
        }
        
        foreach (IHittable enemy in enemies)
        {
            var container = Instantiate(EntityContainerPrefab, EnemyScrollContent.transform);
            container.GetComponent<EntityDisplaySummaryScript>().SetValues(enemy);
        }
    }
    
    
}
