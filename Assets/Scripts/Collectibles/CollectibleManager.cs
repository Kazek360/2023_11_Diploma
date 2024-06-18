using System;
using System.Collections;
using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Collectibles
{
    public class CollectibleManager : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private string tagToSearch;

        private GameObject[] _collectibles;
        private string _currentScene;
        private void Awake()
        {
            _currentScene = SceneManager.GetActiveScene().name;
            _collectibles = GameObject.FindGameObjectsWithTag(tagToSearch);
        }
    
        public void LoadData(GameData gameData)
        {
            if (gameData.collectiblesState.ContainsKey(_currentScene))
            {
                foreach (var collectible in _collectibles)
                {
                    if (gameData.collectiblesState[_currentScene].ContainsKey(collectible.name))
                    {
                        collectible.SetActive(gameData.collectiblesState[_currentScene][collectible.name]);
                    }
                }
            }
        }
        public void SaveData(ref GameData gameData)
        {
            if (!gameData.collectiblesState.ContainsKey(_currentScene))
            {
                gameData.collectiblesState.Add(_currentScene, new Dictionary<string, bool>());
            }

            if (_collectibles.Length > 0)
            {
                foreach (var collectible in _collectibles)
                {
                    if (!gameData.collectiblesState[_currentScene].ContainsKey(collectible.name))
                    {
                        gameData.collectiblesState[_currentScene].Add(collectible.name, collectible.activeSelf);
                    }
                    else
                    {
                        gameData.collectiblesState[_currentScene][collectible.name] = collectible.activeSelf;
                    }
                }
            }
        }
    }
}
