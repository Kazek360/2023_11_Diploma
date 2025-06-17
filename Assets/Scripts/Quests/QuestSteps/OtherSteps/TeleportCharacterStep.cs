using UnityEngine;

namespace Quests.QuestSteps.OtherSteps
{
    public class TeleportCharacterStep : BaseQuestStep
    {
        [SerializeField] private string objectToTeleportName;
        private GameObject _objectToTeleport;
    
        [SerializeField] private string placeToTeleportName;
        private GameObject _placeToTeleport;
        
        protected override void OnEnable()
        {
            base.OnEnable();

            _objectToTeleport = GameObject.Find(objectToTeleportName);
            _placeToTeleport = GameObject.Find(placeToTeleportName);
        }

        private void Start()
        {
            _objectToTeleport.transform.position = _placeToTeleport.transform.position;
            FinishAndDelete();
        }
    }
}