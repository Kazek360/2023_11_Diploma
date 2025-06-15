using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    public interface IInteractable
    {
        public void TriggerInteraction();
        public Vector3 GetCoordinates();
        public void ShowIndicator();
        public void HideIndicator();
    }
}

