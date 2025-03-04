using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Collectibles
{
    public class CollectibleManager : MonoBehaviour, ISavable
    {
        [SerializeField] private string tagToSearch;
        
        private GameObject[] _collectibles;
        private List<string> _collectibleNames = new List<string>();
        private List<bool> _collectibleStates = new List<bool>();
        private string _currentScene;
        private void Awake()
        {
            _currentScene = SceneManager.GetActiveScene().name;
            _collectibles = GameObject.FindGameObjectsWithTag(tagToSearch);
            foreach (var collectible in _collectibles)
            {
                _collectibleNames.Add(collectible.name);
                _collectibleStates.Add(collectible.activeSelf);
            }
        }
    
        private void OnEnable()
        {
            SaveSystemEvents.OnFromFileLoad += LoadData;
            SaveSystemEvents.OnBetweenScenesLoad += LoadData;
        }

        private void OnDisable()
        {
            SaveSystemEvents.OnFromFileLoad -= LoadData;
            SaveSystemEvents.OnBetweenScenesLoad -= LoadData;
        }
        
        private void LoadData(GameData gameData)
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
                for (var i = 0; i < _collectibleNames.Count; i++)
                {
                    if (!gameData.collectiblesState[_currentScene].ContainsKey(_collectibleNames[i]))
                    {
                        gameData.collectiblesState[_currentScene].Add(_collectibleNames[i], _collectibleStates[i]);
                    }
                    else
                    {
                        gameData.collectiblesState[_currentScene][_collectibleNames[i]] = _collectibleStates[i];
                    }
                }
            }
        }
    }
}