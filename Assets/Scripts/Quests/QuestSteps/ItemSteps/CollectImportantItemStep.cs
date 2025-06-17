using InteractionSystem.Collectibles;
using UnityEngine;

namespace Quests.QuestSteps.ItemSteps
{
    public class CollectImportantItemStep : CollectItemStep
    {
        private GameObject _objectToEnable;
        private void Start()
        {
            _objectToEnable = GameObject.Find(itemID);
            var spriteRenderer = _objectToEnable.GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            color.a = 1;
            spriteRenderer.color = color;
            var collectible = _objectToEnable.GetComponent<Collectible>();
            collectible.EnableInteraction();
        }
        
        protected override void IncreaseProgress(string id)
        {
            if (itemID == id)
            {
                FinishAndDelete();
            } 
        }
    }
}