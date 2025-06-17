using UnityEngine;
using UnityEngine.Video;

namespace StorySystems.Cutscenes
{
    [CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscenes/Cutscene")]
    public class Cutscene : ScriptableObject
    {
        [SerializeField] public VideoClip clip;
    }
}
