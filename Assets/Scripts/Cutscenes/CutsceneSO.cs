using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscenes/Cutscene")]
public class CutsceneSO : ScriptableObject
{
    [SerializeField] public List<Sprite> cutsceneImages;
}
