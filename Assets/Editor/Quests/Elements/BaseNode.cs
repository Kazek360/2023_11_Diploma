using System;
using System.Collections.Generic;
using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Quests.Elements
{
    public class BaseNode : Node
    {
        public string ID { get; set; }
        public QuestGraphView graphView;
        public QuestGroup group;
        
        public string fileName;
        public List<string> connectedNodes;
        
        protected Color defaultBackgroundColor = new Color(80f / 255f, 80f / 255f, 80f / 255f);
        
        public virtual void Initialize(string nodeName, QuestGraphView questGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            graphView = questGraphView;
            fileName = nodeName;
            connectedNodes = new List<string>();
            SetPosition(new Rect(position, Vector2.zero));
            extensionContainer.AddToClassList("quest-node__extension-container");
            mainContainer.AddToClassList("quest-node__main-container");
        }
        
        public virtual void Draw()
        {
            DrawTitle();
        }
        
        private void DrawTitle()
        {
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
                
                graphView.RemoveNode(this);
                fileName = callback.newValue;
                graphView.AddNode(this);
                
            });
            fileNameTextField.AddToClassList("quest-node__filename-textfield");
            titleContainer.Insert(0, fileNameTextField);
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
        
        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
        
        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }
    }
}