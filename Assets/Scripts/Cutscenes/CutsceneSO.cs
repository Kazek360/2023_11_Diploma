using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscenes/Cutscene")]
public class CutsceneSO : ScriptableObject
{
    [SerializeField] public VideoClip clip;
}
