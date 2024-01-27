using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityManager : MonoBehaviour
{
    private CombatSystem_2 _combatSystem2;
    [SerializeField] private List<GameObject> turnOrder;
    
    private List<IHittable> _turnOrder;
    private List<UnitCombat> _combatUnits;

    [SerializeField] private int enemiesToDefeat;
    [SerializeField] private int sceneToOpenAfterVictory;
    private int _enemiesDefeated;

    private int _currentEntity;
    // Start is called before the first frame update
    void Start()
    {
        _combatSystem2 = GetComponent<CombatSystem_2>();
        _currentEntity = -1;
        _turnOrder = new List<IHittable>();
        _combatUnits = new List<UnitCombat>();
        _enemiesDefeated = 0;
        foreach (var entity in turnOrder)
        {
            _turnOrder.Add(entity.GetComponent<IHittable>());

            var unit = entity.GetComponent<UnitCombat>();
            _combatSystem2.SetEntityToGrid(unit);
            _combatUnits.Add(unit);
        }
        NextTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        _combatSystem2.ClearGrid();
        StartCoroutine(NextTurnCoroutine());
    }

    private IEnumerator NextTurnCoroutine()
    {
        _currentEntity = (_currentEntity + 1) % _turnOrder.Count;
        yield return new WaitForSeconds(2);
        _combatSystem2.SetCurrentUnit(_combatUnits[_currentEntity]);
        _turnOrder[_currentEntity].ProcessTurn();
    }

    public List<IHittable> GetPlayers()
    {
        List<IHittable> playerEntities = new List<IHittable>();
        foreach (var entity in turnOrder)
        {
            if (entity.CompareTag("Player"))
            {
                playerEntities.Add(entity.GetComponent<IHittable>());
            }
        }

        return playerEntities;
    }

    public List<IHittable> GetEnemies()
    {
        List<IHittable> enemyEntities = new List<IHittable>();
        foreach (var entity in turnOrder)
        {
            if (entity.CompareTag("Enemy"))
            {
                enemyEntities.Add(entity.GetComponent<IHittable>());
                //Debug.Log(entity.name);
            }
        }

        return enemyEntities;
    }

    public void OnEnemyDefeated()
    {
        _enemiesDefeated++;
        if (_enemiesDefeated >= enemiesToDefeat)
        {
            SceneManager.LoadScene(sceneToOpenAfterVictory);
        }
    }
}
