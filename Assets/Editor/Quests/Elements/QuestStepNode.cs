using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Elements
{
    public class QuestStepNode : BaseNode
    {
        public Port outputPort;
        
        public string description;
        public int repeatAmount;
        public GameObject prefab;
        
        public override void Initialize(string nodeName, QuestGraphView questGraphView, Vector2 position)
        {
            base.Initialize(nodeName, questGraphView, position);
            description = "";
            repeatAmount = 0;
        }

        public override void Draw()
        {
            base.Draw();
            DrawPorts();
            DrawDescription();
            DrawRepeatAmount();
            DrawStepPrefab();
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
        
        private void DrawRepeatAmount()
        {
            var repeatContainer = new VisualElement();
            repeatContainer.AddToClassList("quest-node__custom-container");
            var repeatFoldout = QuestGraphUtilities.CreateFoldout("Repeat Amount");
            var amountIntegerField = QuestGraphUtilities.CreateIntegerField(repeatAmount, null,
                callback =>
                {
                    repeatAmount = callback.newValue;
                });
            amountIntegerField.AddToClassList("quest-node__integer-field");
            repeatFoldout.Add(amountIntegerField);
            repeatContainer.Add(repeatFoldout);
            extensionContainer.Add(repeatContainer);
        }
        
        private void DrawStepPrefab()
        {
            var prefabContainer = new VisualElement();
            prefabContainer.AddToClassList("quest-node__custom-container");
            var prefabFoldout = QuestGraphUtilities.CreateFoldout("Prefab");
            var prefabObjectField = new ObjectField
            {
                value = prefab,
                objectType = typeof(GameObject), 
                allowSceneObjects = false 
            };
            prefabObjectField.RegisterValueChangedCallback(callback =>
            {
                if (callback.newValue is GameObject selectedObject)
                {
                    prefab = selectedObject;
                }
            });
            prefabFoldout.Add(prefabObjectField);
            prefabContainer.Add(prefabFoldout);
            extensionContainer.Add(prefabContainer);
        }
    }
}