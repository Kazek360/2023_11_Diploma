using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatSummaryScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ResultText;
    [SerializeField] private GameObject PlayerScrollContent;
    [SerializeField] private GameObject EnemyScrollContent;
    [SerializeField] private GameObject EntityContainerPrefab;

    public void SetValues(bool IsVictory, List<IHittable> players, List<IHittable> enemies)
    {
        if (IsVictory)
        {
            ResultText.text = "VICTORY";
        }
        else
        {
            ResultText.text = "DEFEAT";
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
