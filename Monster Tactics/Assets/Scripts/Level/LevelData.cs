using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Level
{
    [UnityEngine.CreateAssetMenu(fileName = "New Level", menuName = "Monster Tactics/Level", order = 0)]
    public class LevelData : SerializedScriptableObject
    {
        public Dictionary<Vector2Int, QuadTile> Tiles;
        // list of characters/enemies/friendlies
    }
}