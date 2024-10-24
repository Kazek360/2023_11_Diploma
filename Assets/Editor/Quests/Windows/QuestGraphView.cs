using System.Collections.Generic;
using Editor.Quests.Elements;
using Editor.Quests.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Windows
{
    public class QuestGraphView : GraphView
    {
        private Dictionary<string, List<QuestNode>> _ungroupedNodes;
        private Dictionary<Group, Dictionary<string, List<QuestNode>>> _groupedNodes;
        
        public QuestGraphView()
        {
            _ungroupedNodes = new Dictionary<string, List<QuestNode>>();
            _groupedNodes = new Dictionary<Group, Dictionary<string, List<QuestNode>>>();
            AddManipulators();
            AddGridBackground();
            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
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
                _ungroupedNodes.Add(nodeName, new List<QuestNode> { node });
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

        public void AddGroupedNode(QuestNode node, Group group)
        {
            var nodeName = node.fileName;
            node.group = group;
            if (!_groupedNodes.ContainsKey(group))
            {
                _groupedNodes.Add(group, new Dictionary<string, List<QuestNode>>());
            }

            if (!_groupedNodes[group].ContainsKey(nodeName))
            {
                _groupedNodes[group].Add(nodeName, new List<QuestNode>() { node });
                return;
            }
            
            _groupedNodes[group][nodeName].Add(node);
            var errorColor = new Color(200f / 255f, 1f / 255f, 1f / 255f);
            node.SetErrorStyle(errorColor);
            
            if (_groupedNodes[group][nodeName].Count == 2)
            {
                _groupedNodes[group][nodeName][0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveGroupedNode(QuestNode node, Group group)
        {
            var nodeName = node.fileName;
            node.group = null;

            _groupedNodes[group][nodeName].Remove(node);
            node.ResetStyle();

            if (_groupedNodes[group][nodeName].Count == 1)
            {
                _groupedNodes[group][nodeName][0].ResetStyle();
                return;
            }

            if (_groupedNodes[group][nodeName].Count == 0)
            {
                _groupedNodes[group].Remove(nodeName);
                if (_groupedNodes[group].Count == 0)
                {
                    _groupedNodes.Remove(group);
                }
            }
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
                    if (node.group != null)
                    {
                        node.group.RemoveElement(node);
                    }
                    RemoveUngroupedNode(node);
                    RemoveElement(node);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (var element in elements)
                {
                    if (element is not QuestNode)
                    {
                        continue;
                    }

                    var node = (QuestNode)element;
                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, group);
                }
            };
        }
        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (var element in elements)
                {
                    if (element is not QuestNode)
                    {
                        continue;
                    }

                    var node = (QuestNode)element;
                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }
    }
}

