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
            var hittable = entity.GetComponent<IHittable>();
            _turnOrder.Add(hittable);

            var unit = entity.GetComponent<UnitCombat>();
            _combatSystem2.SetEntityToGrid(unit, hittable.IsMaterial());
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
        if (_turnOrder[_currentEntity].IsDead())
        {
            StartCoroutine(NextTurnCoroutine());
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            _combatSystem2.SetCurrentUnit(_combatUnits[_currentEntity]);
            _turnOrder[_currentEntity].ProcessTurn();
        }
    }

    public List<IHittable> GetMaterialPlayers()
    {
        List<IHittable> playerEntities = new List<IHittable>();
        if (_turnOrder is null)
        {
            _turnOrder = new List<IHittable>();
            foreach (var entity in turnOrder)
            {
                var hittable = entity.GetComponent<IHittable>();
                _turnOrder.Add(hittable);
            }
        }
        foreach (var entity in _turnOrder)
        {
            if (entity.GetGameObject().CompareTag("Player") && entity.IsMaterial() && !entity.IsDead())
            {
                playerEntities.Add(entity);
            }
        }

        return playerEntities;
    }

    public List<IHittable> GetMaterialEnemies()
    {
        List<IHittable> enemyEntities = new List<IHittable>();
        if (_turnOrder is null)
        {
            _turnOrder = new List<IHittable>();
            foreach (var entity in turnOrder)
            {
                var hittable = entity.GetComponent<IHittable>();
                _turnOrder.Add(hittable);
            }
        }
        foreach (var entity in _turnOrder)
        {
            if (entity.GetGameObject().CompareTag("Enemy") && entity.IsMaterial() && !entity.IsDead())
            {
                enemyEntities.Add(entity);
                //Debug.Log(entity.name);
            }
        }

        return enemyEntities;
    }
    
    public List<IHittable> GetImmaterialPlayers()
    {
        List<IHittable> playerEntities = new List<IHittable>();
        if (_turnOrder is null)
        {
            _turnOrder = new List<IHittable>();
            foreach (var entity in turnOrder)
            {
                var hittable = entity.GetComponent<IHittable>();
                _turnOrder.Add(hittable);
            }
        }
        foreach (var entity in _turnOrder)
        {
            if (entity.GetGameObject().CompareTag("Player") && !entity.IsMaterial() && !entity.IsDead())
            {
                playerEntities.Add(entity);
            }
        }

        return playerEntities;
    }

    public List<IHittable> GetImmaterialEnemies()
    {
        List<IHittable> enemyEntities = new List<IHittable>();
        if (_turnOrder is null)
        {
            _turnOrder = new List<IHittable>();
            foreach (var entity in turnOrder)
            {
                var hittable = entity.GetComponent<IHittable>();
                _turnOrder.Add(hittable);
            }
        }
        foreach (var entity in _turnOrder)
        {
            if (entity.GetGameObject().CompareTag("Enemy") && !entity.IsMaterial() && !entity.IsDead())
            {
                enemyEntities.Add(entity);
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

    public void ShowEntities(bool showMaterial)
    {
        foreach (var entity in _turnOrder)
        {
            if (entity.IsMaterial() == showMaterial)
            {
                entity.Activate(true);
            }
            else
            {
                entity.Activate(false);
            }
        }
    }

    public void PlayerDied()
    {
        //TODO: EXIT
        Debug.Log("AAAAAA");
    }
}
