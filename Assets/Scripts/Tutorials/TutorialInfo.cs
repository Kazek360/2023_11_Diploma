using UnityEngine;

namespace Tutorials
{
    [CreateAssetMenu(fileName = "Tutorials", menuName = "ScriptableObjects/Tutorials")]
    public class TutorialInfo : ScriptableObject
    {
        public string title;
        public string description;
    }
}