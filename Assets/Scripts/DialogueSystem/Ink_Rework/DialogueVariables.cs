using System.Collections.Generic;
using System.IO;
using Ink;
using Ink.Runtime;

namespace DialogueSystem.Ink_Rework
{
    public class DialogueVariables
    {
        public Dictionary<string, Object> _variablesDictionary;

        public DialogueVariables(string globalsFilePath)
        {
            var compiler = new Compiler(File.ReadAllText(globalsFilePath));
            var globalVariableStory = compiler.Compile();

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

