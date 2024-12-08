using UnityEngine;

namespace Tutorials
{
    [CreateAssetMenu(fileName = "Tutorials", menuName = "ScriptableObjects/Tutorials")]
    public class TutorialInfo : ScriptableObject
    {
        public string title;
        [TextArea] public string description;
    }
}