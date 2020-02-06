using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace Level
{
    [ExecuteInEditMode]
    public class QuadTileMap : SerializedMonoBehaviour
    {
        [SerializeField]
        private Dictionary<Vector2Int, QuadTile> Level = new Dictionary<Vector2Int, QuadTile>();
        public Dictionary<Vector2Int, QuadTile> GetTiles() => Level;

        public Vector3 GetPositionClosestTo(Vector3 position) =>
            new Vector3(Mathf.Round(position.x), 0.0f, Mathf.Round(position.z));

        public void AddTile(Vector3 position, QuadTile newTile)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                DestroyImmediate(newTile.gameObject);
                return;
            }

            Level.Add(position.ToVector2IntXZ(), newTile);
        }

        public void RemoveTile(Vector3 position)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                QuadTile tile = Level[position.ToVector2IntXZ()];
                Level.Remove(position.ToVector2IntXZ());
                DestroyImmediate(tile.gameObject);
            }
        }

        public void ReplaceTile(Vector3 position, QuadTileData brush, float height)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                Level[position.ToVector2IntXZ()].Init(brush, height);
            }
        }
    }
}