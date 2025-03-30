using System.Collections.Generic;
using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using Quests.ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Elements
{
    public class QuestNode : BaseNode
    {
        public Port inputPort;   
        public Port outputQuestPort;
        
        public Port outputQuestStepPort;
        public List<string> connectedFirstStepNodes;
        
        public List<FlagInfo> flags;
        public string displayName;
        public QuestCategory category;
        public string description;
        public int experience;
        
        public override void Initialize(string nodeName, QuestGraphView questGraphView, Vector2 position)
        {
            base.Initialize(nodeName, questGraphView, position);
            displayName = "";
            category = QuestCategory.MainQuest;
            description = "";
            experience = 0;
            flags = new List<FlagInfo>();
        }

        public override void Draw()
        {
            base.Draw();
            DrawPorts();
            DrawFlags();

            //Display Name
            var displayNameContainer = new VisualElement();
            displayNameContainer.AddToClassList("quest-node__custom-container");
            var displayNameFoldout = QuestGraphUtilities.CreateFoldout("Display Name");
            var displayNameTextField = QuestGraphUtilities.CreateTextField(displayName, null, 
                callback =>
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
            var categorySelector = QuestGraphUtilities.CreateEnumField(category, null, 
                callback =>
            {
                category = (QuestCategory) callback.newValue;
            });
            categorySelector.AddToClassList("quest-node__enum-field");
            categoryFoldout.Add(categorySelector);
            categoryContainer.Add(categoryFoldout);
            extensionContainer.Add(categoryContainer);
            
            DrawDescription();
            DrawExperienceField();

            //Rewards
            var rewardsContainer = new VisualElement();
            rewardsContainer.AddToClassList("quest-node__custom-container");
            var rewardsFoldout = QuestGraphUtilities.CreateFoldout("Rewards");
            var rewardsButtonContainer = 
                QuestGraphUtilities.CreateObjectFieldListManipulatorButtons<FlagInfo>(rewardsFoldout, new List<FlagInfo>());
            rewardsFoldout.Add(rewardsButtonContainer);
            rewardsContainer.Add(rewardsFoldout);
            extensionContainer.Add(rewardsContainer);
            
            RefreshExpandedState();
        }

        private void DrawPorts()
        {
            //Input Port
            inputPort = this.CreatePort("Required Quest", Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            //Output Port
            outputQuestPort = this.CreatePort("Next Quests", Direction.Output, Port.Capacity.Multi);
            outputQuestStepPort = this.CreatePort("First Step", Direction.Output, Port.Capacity.Multi);
            outputContainer.Add(outputQuestPort);
            outputContainer.Add(outputQuestStepPort);
        }

        private void DrawFlags()
        {
            var flagsContainer = new VisualElement();
            flagsContainer.AddToClassList("quest-node__custom-container");
            var flagsFoldout = QuestGraphUtilities.CreateFoldout("Flags");
            
            var flagsButtonContainer = 
                QuestGraphUtilities.CreateObjectFieldListManipulatorButtons(flagsFoldout, flags);
            flagsFoldout.Add(flagsButtonContainer);
            
            foreach (var flag in flags)
            {
                var flagObjectField = QuestGraphUtilities.CreateObjectField(flags, flag);
                flagsFoldout.Add(flagObjectField);
            }
            
            flagsContainer.Add(flagsFoldout);
            extensionContainer.Add(flagsContainer);
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

        private void DrawExperienceField()
        {
            var experienceContainer = new VisualElement();
            experienceContainer.AddToClassList("quest-node__custom-container");
            var experienceFoldout = QuestGraphUtilities.CreateFoldout("Experience");
            var experienceIntegerField = QuestGraphUtilities.CreateIntegerField(experience, null,
                callback =>
                {
                    experience = callback.newValue;
                });
            experienceIntegerField.AddToClassList("quest-node__integer-field");
            experienceFoldout.Add(experienceIntegerField);
            experienceContainer.Add(experienceFoldout);
            extensionContainer.Add(experienceContainer);
        }
    }
}

