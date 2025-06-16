using System;
using UnityEngine;

namespace Other
{
    public class Nestor : FadeInOutObject
    {
        public static event Action<string, bool> OnNestorStateChange;

        public static void ChangeNestorState(string nestorID, bool isAppearing)
        {
            OnNestorStateChange?.Invoke(nestorID, isAppearing);
        }
        
        [SerializeField] private string id;
        
        private void OnEnable()
        {
            OnNestorStateChange += CheckID;
        }

        private void OnDisable()
        {
            OnNestorStateChange -= CheckID;
        }

        private void CheckID(string nestorID, bool isAppearing)
        {
            StopAllCoroutines();
            if (nestorID.Equals(id))
            {
                if (isAppearing)
                {
                    StartCoroutine(FadeIn());
                }
                else
                {
                    StartCoroutine(FadeOut());
                }
            }
        }
    }
}