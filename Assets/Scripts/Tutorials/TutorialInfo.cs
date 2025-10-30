using UnityEngine;

namespace Tutorials
{
    [CreateAssetMenu(fileName = "Tutorials", menuName = "ScriptableObjects/Tutorials")]
    public class TutorialInfo : ScriptableObject
    {
        [Header("English")]
        public string titleEnglish;
        [TextArea] public string descriptionEnglish;
        
        [Header("Polish")]
        public string titlePolish;
        [TextArea] public string descriptionPolish;
    }
}