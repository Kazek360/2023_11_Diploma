using System;
using System.Collections.Generic;
using Editor.Quests.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Editor.Quests.Utilities
{
    public static class QuestGraphUtilities
    {
        public static TextField CreateTextField(string value = null, string label = null, 
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            var textfield = new TextField
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
        
        public static EnumField CreateEnumField(Enum value, string label = null, 
            EventCallback<ChangeEvent<Enum>> onValueChanged = null)
        {

            var enumField = new EnumField(label, value);
            
            if (onValueChanged != null)
            {
                enumField.RegisterValueChangedCallback(onValueChanged);
            }
            return enumField;
        }
        
        public static IntegerField CreateIntegerField(int value, string label = null, 
            EventCallback<ChangeEvent<int>> onValueChanged = null)
        {
            var integerField = new IntegerField
            {
                value = value,
                label = label
            };
            
            
            if (onValueChanged != null)
            {
                integerField.RegisterValueChangedCallback(onValueChanged);
            }
            return integerField;
        }
        
        public static ObjectField CreateObjectField<T>(List<T> list, Object value = null) where T : Object
        {
            var objectField = new ObjectField
            {
                value = value,
                objectType = typeof(T), 
                allowSceneObjects = false 
            };
            
            objectField.RegisterValueChangedCallback(callback =>
            {
                if (callback.previousValue != null)
                {
                    if (callback.previousValue is T previousObject)
                    {
                        list.Remove(previousObject);
                    }
                }
                if (callback.newValue is T selectedObject)
                {
                    list.Add(selectedObject);
                }
            });
            return objectField;
        }
        
        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            var foldout = new Foldout
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

        public static VisualElement CreateObjectFieldListManipulatorButtons<T>(Foldout foldout, List<T> list) where T : Object
        {
            var buttonContainer = new VisualElement
            {
                style = { flexDirection = FlexDirection.Row }
            };
            
            var buttonPlus = CreateButton("+", () =>
            {
                var objectField = CreateObjectField(list);
                objectField.AddToClassList("quest-node__object-field");
                foldout.Add(objectField);
            });
            buttonPlus.AddToClassList("quest-node__button");
            var buttonMinus = CreateButton("-", () =>
            {
                if (foldout.childCount > 1)
                {
                    foldout.RemoveAt(foldout.childCount - 1);
                    if (list.Count > 0)
                    {
                        list.RemoveAt(list.Count - 1);
                    }
                }
            });
            buttonMinus.AddToClassList("quest-node__button");
            buttonContainer.Add(buttonPlus);
            buttonContainer.Add(buttonMinus);
            return buttonContainer;
        }
        
        public static Port CreatePort(this Node node, string portName, 
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
