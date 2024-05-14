using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using Object = Ink.Runtime.Object;

namespace DialogueSystem.Managers
{
    public class DialogueVariables
    {
        public Dictionary<string, Object> _variablesDictionary { get; private set; }

        private Story _globalVariableStory;
        public DialogueVariables(TextAsset loadGlobalJson)
        {
            _globalVariableStory = new Story(loadGlobalJson.text);

            _variablesDictionary = new Dictionary<string, Object>();

            foreach (var name in _globalVariableStory.variablesState)
            {
                var value = _globalVariableStory.variablesState.GetVariableWithName(name);
                _variablesDictionary.Add(name, value);
            }
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
            if (_variablesDictionary.ContainsKey(name))
            {
                _variablesDictionary.Remove(name);
                _variablesDictionary.Add(name, value);
            }
        }

        private void VariableToStory(Story story)
        {
            foreach (var variable in _variablesDictionary)
            {
                story.variablesState.SetGlobal(variable.Key, variable.Value);
            }
        }

        public string SaveVariables()
        {
            if (_globalVariableStory is not null)
            {
                VariableToStory(_globalVariableStory);
                return _globalVariableStory.state.ToJson();
            }

            return null;
        }
        public void LoadVariables(TextAsset loadGlobalJson, string variablesString)
        {
            _globalVariableStory = new Story(loadGlobalJson.text);
            
            _globalVariableStory.state.LoadJson(variablesString);

            _variablesDictionary = new Dictionary<string, Object>();

            foreach (var name in _globalVariableStory.variablesState)
            {
                var value = _globalVariableStory.variablesState.GetVariableWithName(name);
                _variablesDictionary.Add(name, value);
            }
        }
    }
}

