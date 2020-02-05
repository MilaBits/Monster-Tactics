using Sirenix.OdinInspector;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu]
    public class TileData : ScriptableObject
    {
        [AssetList(Path = "/Sprites/Tiles/" ), PreviewField(ObjectFieldAlignment.Center)]
        public Sprite top;

        [AssetList(Path = "/Sprites/Tiles/"), PreviewField(ObjectFieldAlignment.Center)]
        public Sprite sides;

        [Header("Stats")]
        [Range(0, 5)]
        public int difficulty = 1;
    }
}