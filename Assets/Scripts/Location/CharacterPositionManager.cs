using System;
using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Location
{
    public class CharacterPositionManager : Singleton<CharacterPositionManager>, IDataPersistence
    {
        [SerializeField] private string tagToSearchNpc;
        private string _currentScene;
        private GameObject _player;
        private GameObject[] _npcs;

        protected override void Awake()
        {
            base.Awake();
            _currentScene = SceneManager.GetActiveScene().name;

            _player = GameObject.FindGameObjectWithTag("Player");
            _npcs = GameObject.FindGameObjectsWithTag(tagToSearchNpc);
        }

        private void LoadNpcPosition(GameData gameData)
        {
            if (gameData.charactersPosition.ContainsKey(_currentScene))
            {
                foreach (var npc in _npcs)
                {
                    if (gameData.charactersPosition[_currentScene].ContainsKey(npc.name))
                    {
                        var characterPosition = gameData.charactersPosition[_currentScene][npc.name];
                        npc.transform.position = new Vector3(characterPosition.Item1, characterPosition.Item2);
                    }
                }
            }
        }

        public void InitializeOnNewGame()
        {
        }

        public void LoadDataFromSaveFile(GameData gameData)
        {
            var playerPosition = gameData.playerPosition;
            _player.transform.position = new Vector3(playerPosition.Item1, playerPosition.Item2);
            LoadNpcPosition(gameData);
        }

        public void LoadDataBetweenScenes(GameData gameData)
        {
            LoadNpcPosition(gameData);
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.currentScene = _currentScene;
            gameData.saveDate = DateTime.Now.ToString("dd.MM.yy HH:mm");
            
            var playerPosition = _player.transform.position;
            gameData.playerPosition = new Tuple<float, float>(playerPosition.x, playerPosition.y);

            if (!gameData.charactersPosition.ContainsKey(_currentScene))
            {
                gameData.charactersPosition.Add(_currentScene, new Dictionary<string, Tuple<float, float>>());
            }
            
            if (_npcs.Length > 0)
            {
                foreach (var character in _npcs)
                {
                    var characterPosition = character.transform.position;
                    if (!gameData.charactersPosition[_currentScene].ContainsKey(character.name))
                    {
                        gameData.charactersPosition[_currentScene].Add(character.name,
                            new Tuple<float, float>(characterPosition.x, characterPosition.y));
                    }
                    else
                    {
                        gameData.charactersPosition[_currentScene][character.name] =
                            new Tuple<float, float>(characterPosition.x, characterPosition.y);
                    }
                }
            }
        }
    }
}
