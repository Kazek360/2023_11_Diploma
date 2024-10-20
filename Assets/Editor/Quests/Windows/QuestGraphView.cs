using System.Collections.Generic;
using Editor.Quests.Elements;
using Editor.Quests.Utilities;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Windows
{
    public class QuestGraphView : GraphView
    {
        private Dictionary<string, List<QuestNode>> _ungroupedNodes;
        
        public QuestGraphView()
        {
            _ungroupedNodes = new Dictionary<string, List<QuestNode>>();
            AddManipulators();
            AddGridBackground();
            OnElementsDeleted();
            AddStyles();
        }

        private void AddGridBackground()
        {
            var gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets("Quests/QuestGraphViewStyles.uss", "Quests/QuestNodeStyles.uss");
        }
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateNodeContextualMenu());
            this.AddManipulator(CreateGroupContextualMenu());
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort == port || startPort.node == port.node || startPort.direction == port.direction)
                {
                    return;
                }
                compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }
        
        private IManipulator CreateNodeContextualMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Node", 
                    actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition)))
                );
            return contextualMenuManipulator;
        }
        private QuestNode CreateNode(Vector2 position)
        {
            var node = new QuestNode();
            node.Initialize(this, position);
            node.Draw();
            AddUngroupedNode(node);
            return node;
        }

        public void AddUngroupedNode(QuestNode node)
        {
            var nodeName = node.fileName;
            if (!_ungroupedNodes.ContainsKey(nodeName))
            {
                var nodeList = new List<QuestNode> { node };
                _ungroupedNodes.Add(nodeName, nodeList);
                return;
            }
            
            _ungroupedNodes[nodeName].Add(node);

            var errorColor = new Color(200f / 255f, 1f / 255f, 1f / 255f);
            node.SetErrorStyle(errorColor);
            if (_ungroupedNodes[nodeName].Count == 2)
            {
                _ungroupedNodes[nodeName][0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveUngroupedNode(QuestNode node)
        {
            var nodeName = node.fileName;

            _ungroupedNodes[nodeName].Remove(node);
            
            node.ResetStyle();

            if (_ungroupedNodes[nodeName].Count == 1)
            {
                _ungroupedNodes[nodeName][0].ResetStyle();
            }

            if (_ungroupedNodes[nodeName].Count == 0)
            {
                _ungroupedNodes.Remove(nodeName);
            }
        }
        
        private IManipulator CreateGroupContextualMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", 
                    actionEvent => AddElement(CreateGroup("Group", actionEvent.eventInfo.localMousePosition)))
            );
            return contextualMenuManipulator;
        }
        private Group CreateGroup(string title, Vector2 mousePosition)
        {
            var group = new Group()
            {
                title = title
            };
            group.SetPosition(new Rect(mousePosition, Vector2.zero));
            return group;
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                var nodesToDelete = new List<QuestNode>();
                foreach (var element in selection)
                {
                    if (element is QuestNode node)
                    {
                        nodesToDelete.Add(node);
                    }
                }

                foreach (var node in nodesToDelete)
                {
                    RemoveUngroupedNode(node);
                    RemoveElement(node);
                }
            };
        }
        
    }
}

