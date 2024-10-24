using System;
using Editor.Quests.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace Editor.Quests.Utilities
{
    public static class QuestGraphUtilities
    {
        public static TextField CreateTextField(string value = null, string label = null, 
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            var textfield = new TextField()
            {
                value = value,
                label = label
            };
            if (onValueChanged != null)
            {
                textfield.RegisterValueChangedCallback(onValueChanged);
            }

            return textfield;
        }

        public static TextField CreateTextArea(string value = null, string label = null, 
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            var textArea = CreateTextField(value, label, onValueChanged);
            textArea.multiline = true;
            return textArea;
        }
        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            var foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };
            
            return foldout;
        }
        
        public static Button CreateButton(string text, Action onClick = null)
        {
            var button = new Button(onClick)
            {
                text = text
            };
            
            return button;
        }
        
        public static Port CreatePort(this QuestNode node, string portName, 
            Direction direction, Port.Capacity capacity, Orientation orientation = Orientation.Horizontal)
        {
            var port = node.InstantiatePort(orientation, direction, capacity, typeof(object));
            port.portName = portName;
            return port;
        }

        public static VisualElement AddStyleSheets(this VisualElement visualElement, params string[] styleSheetsNames)
        {
            foreach (var styleSheetName in styleSheetsNames)
            {
                var styleSheet = (StyleSheet) EditorGUIUtility.Load(styleSheetName);
                visualElement.styleSheets.Add(styleSheet);
            }

            return visualElement;
        }
        public static VisualElement AddStyleClasses(this VisualElement visualElement, params string[] classNames)
        {
            foreach (var className in classNames)
            {
                visualElement.AddToClassList(className);
            }

            return visualElement;
        }
    }
}
