using System;
using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using Shared;
using UnityEngine;

namespace Location
{
    public class CharacterPositionManager : Singleton<CharacterPositionManager>, IDataPersistence
    {
        [SerializeField] private string tagToSearchNpc;
        private GameObject _player;
        private GameObject[] _npcs;
        private string _currentScene;
        
        private string _gatewayID;
        public string GatewayID
        {
            get => _gatewayID;
            set => _gatewayID = value;
        }

        private void Start()
        {
            _currentScene = SceneInfo.Instance.currentScene;
        }

        private void GetData()
        {
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
            GetData();
        }

        public void LoadDataFromSaveFile(GameData gameData)
        {
            GetData();
            var playerPosition = gameData.playerPosition;
            _player.transform.position = new Vector3(playerPosition.Item1, playerPosition.Item2);
            LoadNpcPosition(gameData);
        }

        public void LoadDataBetweenScenes(GameData gameData)
        {
            GetData();
            LoadNpcPosition(gameData);
            
            if (_gatewayID is not null)
            {
                foreach (var gateway in LocationGateway.allGateways)
                {
                    if (gateway.GatewayID.Equals(_gatewayID))
                    {
                        _player.transform.position = gateway.transform.position;
                        _gatewayID = null;
                        break;
                    }
                }
            }
        }

        public void SaveData(ref GameData gameData)
        {
            GetData();
            
            if (_player.gameObject != null)
            {
                var playerPosition = _player.transform.position;
                gameData.playerPosition = new Tuple<float, float>(playerPosition.x, playerPosition.y);
            }
            
            if (_npcs.Length > 0)
            {
                if (_npcs[0].gameObject != null)
                {
                    if (!gameData.charactersPosition.ContainsKey(_currentScene))
                    {
                        gameData.charactersPosition.Add(_currentScene, new Dictionary<string, Tuple<float, float>>());
                    }
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
}
