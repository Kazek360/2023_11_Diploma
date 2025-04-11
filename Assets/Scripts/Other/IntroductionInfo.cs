using UnityEngine;

namespace Other
{
    [CreateAssetMenu(fileName = "Text", menuName = "ScriptableObjects/Text/IntroductionInfo")]
    public class IntroductionInfo: ScriptableObject
    {
        [Header("IntroductionText Text")]
        [SerializeField] public TextAsset introductionText;
    }
}