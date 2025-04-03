using System.Collections.Generic;
using Editor.Quests.Utilities;
using Editor.Quests.Windows;
using Player.Inventory;
using Quests.ScriptableObjects;
using Shared;
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
        
        public string displayName;
        public QuestCategory category;
        public string description;
        public int experience;
        public List<ItemWithAmount> rewards;
        
        public override void Initialize(string nodeName, QuestGraphView questGraphView, Vector2 position)
        {
            base.Initialize(nodeName, questGraphView, position);
            displayName = "";
            category = QuestCategory.MainQuest;
            description = "";
            experience = 0;
            rewards = new List<ItemWithAmount>();
        }

        public override void Draw()
        {
            base.Draw();
            DrawPorts();

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
            DrawRewards();
            
            RefreshExpandedState();
        }

        private void DrawPorts()
        {
            //Input Port
            inputPort = this.CreatePort("Previous Quest", Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            //Output Port
            outputQuestPort = this.CreatePort("Next Quest", Direction.Output, Port.Capacity.Multi);
            outputQuestStepPort = this.CreatePort("First Step", Direction.Output, Port.Capacity.Multi);
            outputContainer.Add(outputQuestPort);
            outputContainer.Add(outputQuestStepPort);
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

        private void DrawRewards()
        {
            var rewardsContainer = new VisualElement();
            rewardsContainer.AddToClassList("quest-node__custom-container");
            var rewardsFoldout = QuestGraphUtilities.CreateFoldout("Rewards");
            var rewardBox =
                QuestGraphUtilities.CreateObjectFieldWithAmountListManipulatorButtons<ItemScriptableObject>(rewardsFoldout, rewards);
            rewardsFoldout.Add(rewardBox);
            
            foreach (var item in rewards)
            {
                var rewardObjectFieldWithAmount = 
                    QuestGraphUtilities.CreateObjectFieldWithAmount<ItemScriptableObject>(rewards, item);
                rewardsFoldout.Add(rewardObjectFieldWithAmount);
            }
            
            rewardsContainer.Add(rewardsFoldout);
            extensionContainer.Add(rewardsContainer);
        }
    }
}

