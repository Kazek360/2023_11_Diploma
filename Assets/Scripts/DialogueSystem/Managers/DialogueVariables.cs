using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using Object = Ink.Runtime.Object;

namespace DialogueSystem.Managers
{
    public class DialogueVariables
    {
        public Dictionary<string, Object> _variablesDictionary { get; private set; }

        public DialogueVariables(TextAsset loadGlobalJson)
        {
            var globalVariableStory = new Story(loadGlobalJson.text);

            _variablesDictionary = new Dictionary<string, Object>();

            foreach (var name in globalVariableStory.variablesState)
            {
                var value = globalVariableStory.variablesState.GetVariableWithName(name);
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
    }
}

