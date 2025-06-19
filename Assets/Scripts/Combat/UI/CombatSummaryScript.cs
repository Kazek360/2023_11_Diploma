using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Controller for combat summary  script
 */
public class CombatSummaryScript : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI ResultText;
    [SerializeField] private GameObject WinText;
    [SerializeField] private GameObject LossText;
    [SerializeField] private GameObject PlayerScrollContent;
    [SerializeField] private GameObject EnemyScrollContent;
    [SerializeField] private GameObject EntityContainerPrefab;
    [SerializeField] private GameObject ProceedWin;
    [SerializeField] private GameObject ProceedLoss;

    private bool _resultsDisplayed = false;

    /**
    * UI setup for combat summary
    * &lt;param name='IsVictory'&gt;whether the player has won an encounter&lt;/param&gt;
    * &lt;param name='players'&gt;list of players in combat&lt;/param&gt;
    * &lt;param name='enemies'&gt;list of enemies in combat&lt;/param&gt;
    */
    public void SetValues(bool IsVictory, List<IHittable> players, List<IHittable> enemies)
    {
        if (_resultsDisplayed)
        {
            return;
        }

        _resultsDisplayed = true;
        Debug.Log("DISPLAY RESULTS");
        if (IsVictory)
        {
            WinText.SetActive(true);
            LossText.SetActive(false);
            ProceedWin.SetActive(true);
            ProceedLoss.SetActive(false);
            //ResultText.text = "ZWYCIĘSTWO";
        }
        else
        {
            WinText.SetActive(false);
            LossText.SetActive(true);
            ProceedWin.SetActive(false);
            ProceedLoss.SetActive(true);
            //ResultText.text = "PORAŻKA";
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
