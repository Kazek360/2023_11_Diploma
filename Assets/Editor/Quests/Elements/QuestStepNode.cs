using System;
using System.Collections.Generic;
using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using Quests.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Elements
{
    public class QuestStepNode : BaseNode
    {
        public Port outputPort;

        public List<FlagInfo> flags;
        public string description;
        public bool isRepeatable;
        public int repeatAmount;
        public GameObject gameObject;
        
        public override void Initialize(string nodeName, QuestGraphView questGraphView, Vector2 position)
        {
            base.Initialize(nodeName, questGraphView, position);
            flags = new List<FlagInfo>();
            description = "";
        }

        public override void Draw()
        {
            base.Draw();
            DrawPorts();
            DrawFlags();
            DrawDescription();
            RefreshExpandedState();
        }
        
        private void DrawPorts()
        {
            //Input Port
            var inputPort = this.CreatePort("Previous Step", Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            //Output Port
            outputPort = this.CreatePort("Next Step", Direction.Output, Port.Capacity.Multi);
            outputContainer.Add(outputPort);
        }
        
        private void DrawFlags()
        {
            var flagsContainer = new VisualElement();
            flagsContainer.AddToClassList("quest-node__custom-container");
            var flagsFoldout = QuestGraphUtilities.CreateFoldout("Flags");
            
            var flagsButtonContainer = 
                QuestGraphUtilities.CreateObjectFieldListManipulatorButtons(flagsFoldout, flags);
            flagsFoldout.Add(flagsButtonContainer);
            
            foreach (var flag in flags)
            {
                var flagObjectField = QuestGraphUtilities.CreateObjectField(flags, flag);
                flagsFoldout.Add(flagObjectField);
            }
            
            flagsContainer.Add(flagsFoldout);
            extensionContainer.Add(flagsContainer);
        }

        
        private void DrawDescription()
        {
            var descriptionContainer = new VisualElement();
            descriptionContainer.AddToClassList("quest-node__custom-container");
            var descriptionFoldout = QuestGraphUtilities.CreateFoldout("Description");
            var descriptionTextArea = QuestGraphUtilities.CreateTextArea(description, null, 
                callback =>
                {
                    description = callback.newValue;
                });
            descriptionTextArea.AddToClassList("quest-node__textfield");
            descriptionFoldout.Add(descriptionTextArea);
            descriptionContainer.Add(descriptionFoldout);
            extensionContainer.Add(descriptionContainer);
        }
    }
}