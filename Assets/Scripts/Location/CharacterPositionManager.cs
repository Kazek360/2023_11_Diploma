using System;
using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Location
{
    public class CharacterPositionManager : MonoBehaviour, IDataPersistence
    {
        private string _currentScene;
        [SerializeField] private string tagToSearchNpc;
        private GameObject _player;
        private GameObject[] _npcs;

        private void Awake()
        {
            _currentScene = SceneManager.GetActiveScene().name;

            _player = GameObject.FindGameObjectWithTag("Player");
            _npcs = GameObject.FindGameObjectsWithTag(tagToSearchNpc);
        }

        public void LoadData(GameData gameData)
        {
            if (gameData.charactersPosition.ContainsKey(_currentScene))
            {
                var playerPosition = gameData.charactersPosition[_currentScene][_player.name];
                _player.transform.position = new Vector3(playerPosition.Item1, playerPosition.Item2);
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

        public void SaveData(ref GameData gameData)
        {
            gameData.currentScene = _currentScene;
            gameData.saveDate = DateTime.Now.ToString("dd.MM.yy HH:mm");

            if (!gameData.charactersPosition.ContainsKey(_currentScene))
            {
                gameData.charactersPosition.Add(_currentScene, new Dictionary<string, Tuple<float, float>>());
            }

            var playerPosition = _player.transform.position;
            if (!gameData.charactersPosition[_currentScene].ContainsKey(_player.name))
            {
                gameData.charactersPosition[_currentScene].Add(_player.name,
                    new Tuple<float, float>(playerPosition.x, playerPosition.y));
            }
            else
            {
                gameData.charactersPosition[_currentScene][_player.name] =
                    new Tuple<float, float>(playerPosition.x, playerPosition.y);
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
