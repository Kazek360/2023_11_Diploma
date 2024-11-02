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
        private QuestEditorWindow _editorWindow;
        private MiniMap _minimap;
        private Dictionary<string, List<QuestNode>> _ungroupedNodes;
        private Dictionary<Group, Dictionary<string, List<QuestNode>>> _groupedNodes;
        private Dictionary<string, List<QuestGroup>> _groups;

        private int _namesErrorErrorAmount;

        public int NamesErrorAmount
        {
            get => _namesErrorErrorAmount;
            set
            {
                _namesErrorErrorAmount = value;
                if (_namesErrorErrorAmount == 0)
                {
                    _editorWindow.EnableSaving();
                }
                else
                {
                    _editorWindow.DisableSaving();
                }
            }
        }

        public QuestGraphView(QuestEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;
            _ungroupedNodes = new Dictionary<string, List<QuestNode>>();
            _groupedNodes = new Dictionary<Group, Dictionary<string, List<QuestNode>>>();
            _groups = new Dictionary<string, List<QuestGroup>>();
            AddManipulators();
            AddMinimap();
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
            this.AddStyleSheets("Quests/QuestGraphStyles.uss", "Quests/QuestNodeStyles.uss");
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

        private void AddMinimap()
        {
            _minimap = new MiniMap
            {
                anchored = true
            };
            
            _minimap.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
            var borderColor = new StyleColor(new Color32(50, 50, 50, 255));
            _minimap.style.borderBottomColor = borderColor;
            _minimap.style.borderTopColor = borderColor;
            _minimap.style.borderRightColor = borderColor;
            _minimap.style.borderLeftColor = borderColor;

            _minimap.SetPosition(new Rect(15, 50, 200, 150));
            
            Add(_minimap);
        }

        public void ToggleMinimap()
        {
            _minimap.visible = !_minimap.visible;
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
                    actionEvent => AddElement(CreateNode("quest", actionEvent.eventInfo.mousePosition)))
                );
            return contextualMenuManipulator;
        }
        public QuestNode CreateNode(string nodeName, Vector2 position, bool shouldDraw = true)
        {
            var node = new QuestNode();
            node.Initialize(nodeName, this, position);
            if (shouldDraw)
            {
                node.Draw();
            }
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
                NamesErrorAmount++;
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
                NamesErrorAmount--;
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
                NamesErrorAmount++;
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
                NamesErrorAmount--;
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
        public QuestGroup CreateGroup(string title, Vector2 mousePosition)
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
        private void AddGroup(QuestGroup group)
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
                NamesErrorAmount++;
                _groups[groupName][0].SetErrorStyle(errorColor);
            }
        }
        
        private void RemoveGroup(QuestGroup group)
        {
            var oldGroupName = group.oldTitle;

            _groups[oldGroupName].Remove(group);
            
            group.ResetStyle();

            if (_groups[oldGroupName].Count == 1)
            {
                NamesErrorAmount--;
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
                questGroup.title = newTitle; 
                
                if (string.IsNullOrEmpty(questGroup.title))
                {
                    if (!string.IsNullOrEmpty(questGroup.oldTitle))
                    {
                        NamesErrorAmount++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(questGroup.oldTitle))
                    {
                        NamesErrorAmount--;
                    }
                }
                
                RemoveGroup(questGroup);
                questGroup.oldTitle = questGroup.title;
                AddGroup(questGroup);
            };
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            
            _groups.Clear();
            _ungroupedNodes.Clear();
            _groupedNodes.Clear();
            NamesErrorAmount = 0;
        }
        
    }
}

