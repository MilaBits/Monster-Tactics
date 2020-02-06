using Sirenix.OdinInspector;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu]
    public class QuadTileData : ScriptableObject
    {
        [AssetList(Path = "/Materials/Tiles/" ), PreviewField(ObjectFieldAlignment.Center)]
        public Material top;

        [AssetList(Path = "/Materials/Tiles/"), PreviewField(ObjectFieldAlignment.Center)]
        public Material sides;

        [Header("Stats")]
        [Range(0, 5)]
        public int difficulty = 1;
    }
}