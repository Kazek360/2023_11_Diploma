using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/**
 * Class managing state of entities in combat as well as their turn order logic
 */
public class EntityManager : MonoBehaviour
{
    private CombatSystem _combatSystem;
    [FormerlySerializedAs("turnOrder")] [SerializeField] private List<GameObject> TurnOrder;
    [SerializeField] private List<GameObject> entityPrefabs;
    
    private List<IHittable> _turnOrder;
    private List<UnitCombat> _combatUnits;

    [FormerlySerializedAs("enemiesToDefeat")] [SerializeField] private int EnemiesToDefeat;
    [FormerlySerializedAs("sceneToOpenAfterVictory")] [SerializeField] private int SceneToOpenAfterVictory;
    [SerializeField] [TextArea] private String entitiesString;
    
    [SerializeField] private int SceneToOpenAfterLoss;
    private int _enemiesDefeated;

    private int _currentEntity;
    // Start is called before the first frame update
    void OnEnable()
    {
        _combatSystem = GetComponent<CombatSystem>();
        TurnOrder = SpawnEntities();
        _currentEntity = -1;
        _turnOrder = new List<IHittable>();
        _combatUnits = new List<UnitCombat>();
        _enemiesDefeated = 0;
        foreach (var entity in TurnOrder)
        {
            var hittable = entity.GetComponent<IHittable>();
            _turnOrder.Add(hittable);

            var unit = entity.GetComponent<UnitCombat>();
            _combatSystem.SetEntityToGrid(unit, hittable.IsMaterial());
            _combatUnits.Add(unit);
        }
        _combatSystem.DisplayQueue(_turnOrder);
        NextTurn();
    }

    public void Start()
    {
        //ShowEntities(true);
    }


    public void NextTurn()
    {
        _combatSystem.ClearGrid();
        StartCoroutine(NextTurnCoroutine());
    }

    /**
    * A coroutine switching current player.
     * TODO: AFTER ANIMATIONS WILL BE ADDED, HERE WAIT FOR THEM TO FINISH
    */  
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
            UpdateDeadEntitiesInQueue();
            _combatSystem.SetCurrentUnit(_combatUnits[_currentEntity], _currentEntity);
            _turnOrder[_currentEntity].ProcessTurn();
        }
    }

    public List<IHittable> GetMaterialPlayers()
    {
        List<IHittable> playerEntities = new List<IHittable>();
        if (_turnOrder is null)
        {
            _turnOrder = new List<IHittable>();
            foreach (var entity in TurnOrder)
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
            foreach (var entity in TurnOrder)
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
            foreach (var entity in TurnOrder)
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
            foreach (var entity in TurnOrder)
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
        if (_enemiesDefeated >= EnemiesToDefeat)
        {
            ShowCombatSummary(true);
        }
    }

    /**
    * Make appropriate entities visible
    * <param name='showMaterial'>Whether displayed entities should be material</param>
    */   
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
        ShowCombatSummary(false);
    }

    public void OnCombatWin()
    {
        SceneManager.LoadScene(SceneToOpenAfterVictory);
    }
    
    public void OnCombatLoss()
    {
        SceneManager.LoadScene(SceneToOpenAfterLoss);
    }
    
    
    /**
    * End combat and pass entities to function displaying results
    * <param name='IsVictory'>whether the player has won an encounter</param>
    */    
    private void ShowCombatSummary(bool IsVictory)
    {
        
        List<IHittable> players = new List<IHittable>();
        foreach (var entity in _turnOrder)
        {
            if (entity.GetGameObject().CompareTag("Player"))
            {
                players.Add(entity);
            }
        }
        
        List<IHittable> enemies = new List<IHittable>();
        
        foreach (var entity in _turnOrder)
        {
            if (entity.GetGameObject().CompareTag("Enemy"))
            {
                enemies.Add(entity);
                //Debug.Log(entity.name);
            }
        }

        _combatSystem.ShowCombatSummary(IsVictory, players, enemies);
    }
    
    public void UpdateDeadEntitiesInQueue() {
        for (int i = 0; i < _turnOrder.Count; i++)
        {
            if (_turnOrder[i].GetCurrentHealth() <= 0)
            {
                _combatSystem.DisableDeadEntityInQueue(i);
            }
        }
    }

    private List<GameObject> SpawnEntities()
    {
        List<GameObject> entityVec = new List<GameObject>();
        String[] entityLines = entitiesString.Split("\n");
        int[][] entities = new int[entityLines.Length][];
        for(int i = 0; i < entityLines.Length; i++)
        {
            entities[i] = new int[4];   //Format: entity_number,x,y
            String[] entityValues = entityLines[i].Split(",");
            entities[i][0] = int.Parse(entityValues[0]);
            entities[i][1] = int.Parse(entityValues[1]);
            entities[i][2] = int.Parse(entityValues[2]);
            if (entityValues.Length > 3)
            {
                entities[i][3] = int.Parse(entityValues[3]);
            }

            GameObject spawned = Instantiate(entityPrefabs[entities[i][0]], gameObject.transform);
            spawned.GetComponent<UnitCombat>().SetGridLocation(entities[i][1], entities[i][2]);
            if (entityValues.Length > 3)
            {
                entities[i][3] = int.Parse(entityValues[3]);
                spawned.GetComponent<IHittable>().SetPartnerEntity(entityVec[entities[i][3]].GetComponent<IHittable>());
                entityVec[entities[i][3]].GetComponent<IHittable>().SetPartnerEntity(spawned.GetComponent<IHittable>());
            }
            entityVec.Add(spawned);
        }
        entityVec.Sort((p1,p2)=>p1.GetComponent<IHittable>().GetAgility().CompareTo(p2.GetComponent<IHittable>().GetAgility()));
        return entityVec;
    }

    public void UpdateEntityHealthbars()
    {
        foreach (var entity in _turnOrder)
        {
            if (entity.GetGameObject().activeInHierarchy)
            {
                entity.ModifyHealth(0);
            }
        }
    }
}
