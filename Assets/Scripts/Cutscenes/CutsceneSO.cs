using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscenes/Cutscene")]
public class CutsceneSO : ScriptableObject
{
    [SerializeField] public Sprite cutsceneImage;
}
