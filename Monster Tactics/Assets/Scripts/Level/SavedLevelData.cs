using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Level
{
    public class SavedLevelData : SerializedScriptableObject
    {
        public Dictionary<Vector2Int, SavedTileData> Tiles = new Dictionary<Vector2Int, SavedTileData>();
        // list of characters/enemies/friendlies

        [Serializable]
        public struct SavedTileData
        {
            public float Height;
            public QuadTileData Data;
        }
    }
}