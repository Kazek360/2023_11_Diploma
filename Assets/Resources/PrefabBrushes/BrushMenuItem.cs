using UnityEngine;
using UnityEditor;

namespace UnityEditor.Tilemaps
{
    static internal partial class AssetCreation
    {
        [MenuItem("Assets/Create/2D/Brushes/Prefab Brush", priority = 0)]
        static void CreatePrefabBrush()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<PrefabBrush>(), "New Prefab Brush.asset");
        }

        [MenuItem("Assets/Create/2D/Brushes/Prefab Random Brush", priority = 1)]
        static void CreatePrefabRandomBrush()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<PrefabRandomBrush>(),
                "New Prefab Random Brush.asset");
        }

        [MenuItem("Assets/Create/2D/Brushes/Random Brush", priority = 2)]
        static void CreateRandomBrush()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<RandomBrush>(), "New Random Brush.asset");
        }
    }
}