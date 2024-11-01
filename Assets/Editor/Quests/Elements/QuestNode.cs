using System;
using System.Collections.Generic;
using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Elements
{
    public class QuestNode : Node
    {
        public string ID { get; set; }
        public QuestGraphView graphView;
        public QuestGroup group;
        public Port inputPort;   
        public Port outputPort;        
        
        public string fileName;
        public string displayName;
        public QuestCategory category;
        public string description;

        public List<QuestNode> nextQuests;
        
        private Color _defaultBackgroundColor = new Color(80f / 255f, 80f / 255f, 80f / 255f);

        public void Initialize(QuestGraphView questGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            graphView = questGraphView;
            fileName = "Quest";
            displayName = "";
            category = QuestCategory.MainQuest;
            description = "";

            nextQuests = new List<QuestNode>();
            
            SetPosition(new Rect(position, Vector2.zero));
            extensionContainer.AddToClassList("quest-node__extension-container");
            mainContainer.AddToClassList("quest-node__main-container");
        }

        public void Draw()
        {
            //Node Title
            var fileNameTextField = QuestGraphUtilities.CreateTextField(fileName, null, callback =>
            {
                var target = (TextField) callback.target;
                target.value = callback.newValue;

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        graphView.NamesErrorAmount++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        graphView.NamesErrorAmount--;
                    }
                }
                
                if (group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    fileName = callback.newValue;
                    graphView.AddUngroupedNode(this);
                    return;
                }

                var currentGroup = group;
                graphView.RemoveGroupedNode(this, group);
                fileName = target.value;
                graphView.AddGroupedNode(this, currentGroup);
                
            });
            fileNameTextField.AddToClassList("quest-node__filename-textfield");
            titleContainer.Insert(0, fileNameTextField);

            //Input Port
            inputPort = this.CreatePort("Required Quest", Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            //Output Port
            outputPort = this.CreatePort("Next Quests", Direction.Output, Port.Capacity.Multi);
            outputContainer.Add(outputPort);
            
            //Display Name
            var displayNameContainer = new VisualElement();
            displayNameContainer.AddToClassList("quest-node__custom-container");
            var displayNameFoldout = QuestGraphUtilities.CreateFoldout("Display Name");
            var displayNameTextField = QuestGraphUtilities.CreateTextField(displayName, null, callback =>
            {
                displayName = callback.newValue;
            });
            displayNameTextField.AddToClassList("quest-node__textfield");
            displayNameFoldout.Add(displayNameTextField);
            displayNameContainer.Add(displayNameFoldout);
            extensionContainer.Add(displayNameContainer);
            
            //Quest Category
            var categoryContainer = new VisualElement();
            categoryContainer.AddToClassList("quest-node__custom-container");
            var categoryFoldout = QuestGraphUtilities.CreateFoldout("Category");
            var categorySelector = QuestGraphUtilities.CreateEnumField(category, null, callback =>
            {
                category = (QuestCategory) callback.newValue;
            });
            categorySelector.AddToClassList("quest-node__enum-field");
            categoryFoldout.Add(categorySelector);
            categoryContainer.Add(categoryFoldout);
            extensionContainer.Add(categoryContainer);
            
            //Quest Description
            var descriptionFoldout = QuestGraphUtilities.CreateFoldout("Description");
            var descriptionTextArea = QuestGraphUtilities.CreateTextArea(description, null, callback =>
            {
                description = callback.newValue;
            });
            descriptionTextArea.AddToClassList("quest-node__textfield");
            descriptionFoldout.Add(descriptionTextArea);
            extensionContainer.Add(descriptionFoldout);
            
            RefreshExpandedState();
        }

        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }
        
        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (port.connected)
                {
                    graphView.DeleteElements(port.connections);
                }
            }
        }
        
        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defaultBackgroundColor;
        }
    }
}

