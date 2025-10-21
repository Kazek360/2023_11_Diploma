using System.Collections.Generic;
using Ink.Runtime;
using Quests.ScriptableObjects;
using SaveSystem.Data;
using UnityEngine;
using Object = Ink.Runtime.Object;

namespace Quests.Flags
{
    public class FlagManager
    {
        private Dictionary<string, Object> _inkVariablesDictionary;
        public Dictionary<string, Object> InkVariablesDictionary => _inkVariablesDictionary;

        private Dictionary<string, bool> _flagDictionary;
        public Dictionary<string, bool> FlagDictionary => _flagDictionary;


        private Story _globalVariableStory;
        public FlagManager(TextAsset loadGlobalJson)
        {
            _globalVariableStory = new Story(loadGlobalJson.text);

            _inkVariablesDictionary = new Dictionary<string, Object>();

            foreach (var name in _globalVariableStory.variablesState)
            {
                var value = _globalVariableStory.variablesState.GetVariableWithName(name);
                _inkVariablesDictionary.Add(name, value);
            }

            _flagDictionary = CreateFlagDictionary();
        }
        public void StartListening(Story story)
        {
            VariableToStory(story);
            story.variablesState.variableChangedEvent += VariableChanged;
        }
        
        public void StopListening(Story story)
        {
            story.variablesState.variableChangedEvent -= VariableChanged;
        }
        
        private void VariableChanged(string name, Object value)
        {
            if (_inkVariablesDictionary.ContainsKey(name))
            {
                _inkVariablesDictionary.Remove(name);
                _inkVariablesDictionary.Add(name, value);
                
                if (value is BoolValue boolVal)
                {
                    var isTriggered = boolVal.value;
                    _flagDictionary[name] = isTriggered;
                }
            }
        }

        private void VariableToStory(Story story)
        {
            foreach (var variable in _inkVariablesDictionary)
            {
                story.variablesState.SetGlobal(variable.Key, variable.Value);
            }
        }

        public bool IsFlagTriggered(string flagName)
        {
            if (FlagDictionary.ContainsKey(flagName))
            {
                Debug.LogWarning($"Ink Variable was not found: {flagName}");
                return false;
            }

            return FlagDictionary[flagName];
        }
        
        private void SetBoolVariableState(string variableName, BoolValue variableValue)
        {
            if (InkVariablesDictionary.ContainsKey(variableName))
            {
                var globalStory = new Story(QuestManager.Instance.LoadGlobals.text);
                StartListening(globalStory);
                globalStory.variablesState.SetGlobal(variableName, variableValue);
                StopListening(globalStory);
            }
        }

        public void SetIntVariableState(string variableName, IntValue variableValue)
        {
            if (InkVariablesDictionary.ContainsKey(variableName))
            {
                var globalStory = new Story(QuestManager.Instance.LoadGlobals.text);
                StartListening(globalStory);
                globalStory.variablesState.SetGlobal(variableName, variableValue);
                StopListening(globalStory);
            }
        }

        public void TriggerFlag(string flagName)
        {
            if (FlagDictionary.ContainsKey(flagName))
            {
                FlagDictionary[flagName] = true;
                SetBoolVariableState(flagName, new BoolValue(true));
            }
        }
        
        public void LoadFlags(Dictionary<string, bool> flags)
        {
            foreach (var pair in flags)
            {
                if (!_flagDictionary.ContainsKey(pair.Key))
                {
                    continue;
                }
                _flagDictionary[pair.Key] = pair.Value;
                SetBoolVariableState(pair.Key, new BoolValue(pair.Value));
            }
        }

        public void SaveFlags(GameData gameData)
        {
            gameData.Flags = _flagDictionary;
        }
        
        private Dictionary<string, bool> CreateFlagDictionary()
        {
            var allFlags = Resources.LoadAll<FlagInfo>("Quests/Flags");
            var idToDictionary = new Dictionary<string, bool>();
            foreach (var flagInfo in allFlags)
            {
                if (idToDictionary.ContainsKey(flagInfo.name))
                {
                    Debug.LogWarning($"Found Duplicate ID: {flagInfo.name}");
                }

                idToDictionary.Add(flagInfo.name, false);
            }

            return idToDictionary;
        }
        
    }
}

