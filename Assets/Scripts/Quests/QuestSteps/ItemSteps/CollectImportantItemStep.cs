using UnityEngine;

namespace Quests.QuestSteps.ItemSteps
{
    public class CollectImportantItemStep : CollectItemStep
    {
        [SerializeField] protected GameObject itemPrefab;
        [SerializeField] protected string spotToInstantiateName;
        private GameObject _spotToInstantiate;
        private void Start()
        {
            _spotToInstantiate = GameObject.Find(spotToInstantiateName);
            Instantiate(itemPrefab, _spotToInstantiate.transform);
        }
    }
}