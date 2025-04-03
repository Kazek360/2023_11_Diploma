using System.Collections.Generic;
using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using Quests.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Elements
{
    public class QuestStepTransitionNode : BaseNode
    {
        public Port outputPort;

        public List<FlagInfo> flags;
        
        public override void Initialize(string nodeName, QuestGraphView questGraphView, Vector2 position)
        {
            base.Initialize(nodeName, questGraphView, position);
            flags = new List<FlagInfo>();
        }

        public override void Draw()
        {
            base.Draw();
            DrawPorts();
            DrawFlags();
            RefreshExpandedState();
        }
        
        private void DrawPorts()
        {
            //Input Port
            var inputPort = this.CreatePort("Previous Step", Direction.Input, Port.Capacity.Single);
            inputContainer.Add(inputPort);
            
            //Output Port
            outputPort = this.CreatePort("Next Step", Direction.Output, Port.Capacity.Single);
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
    }
}