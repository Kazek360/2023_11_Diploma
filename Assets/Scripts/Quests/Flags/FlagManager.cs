using System.Collections.Generic;
using Ink.Runtime;
using SaveSystem.Data;
using UnityEngine;
using Object = Ink.Runtime.Object;

namespace Quests.Flags
{
    public class FlagManager
    {
        private Dictionary<string, Object> _inkVariablesDictionary;
        public Dictionary<string, Object> InkVariablesDictionary => _inkVariablesDictionary;

        private Dictionary<string, Flag> _flagDictionary;
        public Dictionary<string, Flag> FlagDictionary => _flagDictionary;


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
            }
        }

        private void VariableToStory(Story story)
        {
            foreach (var variable in _inkVariablesDictionary)
            {
                story.variablesState.SetGlobal(variable.Key, variable.Value);
            }
        }

        public string GetFlagValue(string flagName)
        {
            Flag flag;
            FlagDictionary.TryGetValue(flagName, out flag);
            if (flag is null)
            {
                Debug.LogWarning($"Ink Variable was not found: {flagName}");
            }

            return flag?.value;
        }
        
        public void SetVariableState(string variableName, Object variableValue)
        {
            if (InkVariablesDictionary.ContainsKey(variableName))
            {
                var globalStory = new Story(QuestManager.Instance.LoadGlobals.text);
                StartListening(globalStory);
                globalStory.variablesState.SetGlobal(variableName, variableValue);
                StopListening(globalStory);
            }
        }

        public void SetFlagValue(string flagName, string flagValue)
        {
            if (FlagDictionary.ContainsKey(flagName))
            {
                FlagDictionary[flagName].value = flagValue;
                SetVariableState(flagName, new StringValue(flagValue));
            }
        }
        
        
        public void LoadFlags(Dictionary<string, Flag> flags)
        {
            foreach (var pair in flags)
            {
                _flagDictionary[pair.Key].flagName = pair.Value.flagName;
                _flagDictionary[pair.Key].value = pair.Value.value;
                SetVariableState(pair.Value.flagName, new StringValue(pair.Value.value));
            }
        }

        public void SaveFlags(GameData gameData)
        {
            gameData.Flags = _flagDictionary;
        }
        
        private Dictionary<string, Flag> CreateFlagDictionary()
        {
            var allFlags = Resources.LoadAll<FlagInfo>("Quests/Flags");
            var idToDictionary = new Dictionary<string, Flag>();
            foreach (var flagInfo in allFlags)
            {
                if (idToDictionary.ContainsKey(flagInfo.name))
                {
                    Debug.LogWarning($"Found Duplicate ID: {flagInfo.name}");
                }

                idToDictionary.Add(flagInfo.name, new Flag(flagInfo.name));
            }

            return idToDictionary;
        }
        
    }
}

