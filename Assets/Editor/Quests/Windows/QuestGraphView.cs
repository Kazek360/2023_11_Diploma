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
        private Dictionary<string, List<QuestGroup>> _groups;
        
        public QuestGraphView()
        {
            _ungroupedNodes = new Dictionary<string, List<QuestNode>>();
            _groupedNodes = new Dictionary<Group, Dictionary<string, List<QuestNode>>>();
            _groups = new Dictionary<string, List<QuestGroup>>();
            AddManipulators();
            AddGridBackground();
            
            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            
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
        
        public void AddGroupedNode(QuestNode node, QuestGroup group)
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
        
        private IManipulator CreateGroupContextualMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", 
                    actionEvent => CreateGroup("Group", actionEvent.eventInfo.localMousePosition))
            );
            return contextualMenuManipulator;
        }
        private Group CreateGroup(string title, Vector2 mousePosition)
        {
            var group = new QuestGroup(title, mousePosition);
            AddGroup(group);
            AddElement(group);

            foreach (var element in selection)
            {
                if (element is QuestNode node)
                {
                    group.AddElement(node);
                }
            }
            
            return group;
        }
        public void AddGroup(QuestGroup group)
        {
            var groupName = group.title;
            if (!_groups.ContainsKey(groupName))
            {
                _groups.Add(groupName, new List<QuestGroup> { group });
                return;
            }
            
            _groups[groupName].Add(group);

            var errorColor = new Color(200f / 255f, 1f / 255f, 1f / 255f);
            group.SetErrorStyle(errorColor);
            if (_groups[groupName].Count == 2)
            {
                _groups[groupName][0].SetErrorStyle(errorColor);
            }
        }
        
        public void RemoveGroup(QuestGroup group)
        {
            var oldGroupName = group.oldTitle;

            _groups[oldGroupName].Remove(group);
            
            group.ResetStyle();

            if (_groups[oldGroupName].Count == 1)
            {
                _groups[oldGroupName][0].ResetStyle();
            }

            if (_groups[oldGroupName].Count == 0)
            {
                _groups.Remove(oldGroupName);
            }
        }
        
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                var groupsToDelete = new List<QuestGroup>();
                var edgesToDelete = new List<Edge>();
                var nodesToDelete = new List<QuestNode>();
                foreach (var element in selection)
                {
                    if (element is QuestNode node)
                    {
                        nodesToDelete.Add(node);
                    }
                    else if (element is QuestGroup group)
                    {
                        groupsToDelete.Add(group);
                    }
                    if (element is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                    }
                }
                
                foreach (var node in nodesToDelete)
                {
                    if (node.group != null)
                    {
                        node.group.RemoveElement(node);
                    }
                    RemoveUngroupedNode(node);
                    node.DisconnectAllPorts();
                    RemoveElement(node);
                }
                
                DeleteElements(edgesToDelete);
                
                foreach (var group in groupsToDelete)
                {
                    var groupNodes = new List<QuestNode>();
                    foreach (var element in group.containedElements)
                    {
                        if (element is QuestNode node)
                        {
                            groupNodes.Add(node);
                        }
                    }
                    group.RemoveElements(groupNodes);
                    RemoveGroup(group);
                    RemoveElement(group);
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
                    
                    var nodeGroup = (QuestGroup)group;
                    var node = (QuestNode)element;
                    
                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, nodeGroup);
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
        
        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                var questGroup = (QuestGroup)group;
                RemoveGroup(questGroup);
                questGroup.oldTitle = newTitle;
                AddGroup(questGroup);
            };
        }
        
    }
}

