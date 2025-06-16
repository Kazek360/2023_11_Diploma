using System.Collections.Generic;
using InteractionSystem.Location;
using SaveSystem;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using Shared;
using UnityEngine;

namespace Location
{
    public class CharacterPositionLoader : Singleton<CharacterPositionLoader>, ISavable, IFromFileLoadable, IBetweenScenesLoadable
    {
        [SerializeField] private string tagToSearchNpc;
        private string _currentScene;
        
        private void OnEnable()
        {
            SaveSystemEvents.OnFromFileLoad += LoadData;
            SaveSystemEvents.OnBetweenScenesLoad += LoadData;
            SaveSystemEvents.OnSave += SaveData;
        }

        private void OnDisable()
        {
            SaveSystemEvents.OnFromFileLoad -= LoadData;
            SaveSystemEvents.OnBetweenScenesLoad -= LoadData;
            SaveSystemEvents.OnSave -= SaveData;
        }
        
        private void Start()
        {
            _currentScene = SceneInfo.Instance.currentScene;
        }
        
        private void LoadNpcPosition(GameData gameData)
        {
            _currentScene = SceneInfo.Instance.currentScene;
            if (gameData.charactersPosition.npcPositions.ContainsKey(_currentScene))
            {
                foreach (var characterPosition in gameData.charactersPosition.npcPositions[_currentScene])
                {
                    var inGameCharacterPosition = CharacterPosition.allPositions.Find(e =>
                        e.characterName == characterPosition.name);

                    inGameCharacterPosition.characterTransform.position = 
                        new Vector3(characterPosition.x, characterPosition.y);
                }
            }
        }

        public void LoadDataFromSaveFile(GameData gameData)
        {
            var playerPosition = gameData.charactersPosition.playerPosition;
            PlayerPosition.instance.transform.position = new Vector3(playerPosition.x, playerPosition.y);
            LoadNpcPosition(gameData);
        }

        public void LoadDataBetweenScenes(GameData gameData)
        {
            LoadNpcPosition(gameData);
            
            if (LocationGateway.IdToUse != null)
            {
                foreach (var gateway in LocationGateway.allGateways)
                {
                    if (gateway.GatewayID.Equals(LocationGateway.IdToUse))
                    {
                        PlayerPosition.instance.playerTransform.position = gateway.transform.position;
                        LocationGateway.IdToUse = null;
                        break;
                    }
                }
            }
        }

        public void LoadData(GameData gameData)
        {
            switch (DataPersistenceManager.Instance.CurrentLoadMode)
            {
                case LoadMode.FromSaveFile:
                    LoadDataFromSaveFile(gameData);
                    break;
                case LoadMode.BetweenScenes:
                    LoadDataBetweenScenes(gameData);
                    break;
            }
        }

        public void SaveData(GameData gameData)
        {
            if (PlayerPosition.instance != null)
            {
                var playerPosition = PlayerPosition.instance.playerTransform.position;
                gameData.charactersPosition.playerPosition = new CharacterPositionData.Position
                {
                    name = "player",
                    x = playerPosition.x,
                    y = playerPosition.y
                };
            }

            if (CharacterPosition.allPositions.Count > 0)
            {
                if (!gameData.charactersPosition.npcPositions.ContainsKey(_currentScene))
                {
                    gameData.charactersPosition.npcPositions.Add(_currentScene, new List<CharacterPositionData.Position>());
                }
            
                foreach (var character in CharacterPosition.allPositions)
                {
                    var characterPosition = character.characterTransform.position;
                    gameData.charactersPosition.npcPositions[_currentScene].Add(new CharacterPositionData.Position
                    {
                        name = character.characterName,
                        x = characterPosition.x,
                        y = characterPosition.y
                    });
                }  
            }
        }
    }
}
