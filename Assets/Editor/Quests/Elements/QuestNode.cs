using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Elements
{
    public class QuestNode : Node
    {
        public QuestGraphView graphView;
        public QuestGroup group;
        public string fileName;
        public string displayName;
        public string description;

        private Color _defaultBackgroundColor = new Color(80f / 255f, 80f / 255f, 80f / 255f);

        public void Initialize(QuestGraphView questGraphView, Vector2 position)
        {
            graphView = questGraphView;
            fileName = "Quest";
            displayName = "";
            description = "";
            
            SetPosition(new Rect(position, Vector2.zero));
            extensionContainer.AddToClassList("quest-node__extension-container");
            mainContainer.AddToClassList("quest-node__main-container");
        }

        public void Draw()
        {
            //Node Title
            var fileNameTextField = QuestGraphUtilities.CreateTextField(fileName, callback =>
            {
                if (group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    fileName = callback.newValue;
                    graphView.AddUngroupedNode(this);
                    return;
                }

                var currentGroup = group;
                graphView.RemoveGroupedNode(this, group);
                fileName = callback.newValue;
                graphView.AddGroupedNode(this, currentGroup);
                
            });
            fileNameTextField.AddToClassList("quest-node__filename-textfield");
            titleContainer.Insert(0, fileNameTextField);

            //Input Port
            inputContainer.Add(this.CreatePort("Required Quest", Direction.Input, Port.Capacity.Multi));
            
            //Output Port
            outputContainer.Add(this.CreatePort("Quest Output", Direction.Output, Port.Capacity.Multi));
            
            //Display Name
            var displayNameContainer = new VisualElement();
            displayNameContainer.AddToClassList("quest-node__custom-container");
            var displayNameFoldout = QuestGraphUtilities.CreateFoldout("Display Name");
            var displayNameTextField = QuestGraphUtilities.CreateTextField(displayName);
            displayNameTextField.AddToClassList("quest-node__textfield");
            displayNameFoldout.Add(displayNameTextField);
            displayNameContainer.Add(displayNameFoldout);
            extensionContainer.Add(displayNameContainer);
            
            //Quest Category
            var categoryContainer = new VisualElement();
            categoryContainer.AddToClassList("quest-node__custom-container");
            var categoryFoldout = QuestGraphUtilities.CreateFoldout("Category");
            var categorySelector = new EnumField(QuestCategory.MainQuest);
            categorySelector.AddToClassList("quest-node__enum-field");
            categoryFoldout.Add(categorySelector);
            categoryContainer.Add(categoryFoldout);
            extensionContainer.Add(categoryContainer);
            
            //Quest Description
            var descriptionFoldout = QuestGraphUtilities.CreateFoldout("Description");
            var descriptionTextField = QuestGraphUtilities.CreateTextArea(description);
            descriptionTextField.AddToClassList("quest-node__textfield");
            descriptionFoldout.Add(descriptionTextField);
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

