using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace Level.OLD
{
    [ExecuteInEditMode]
    public class TileMap : SerializedMonoBehaviour
    {
        [SerializeField]
        private Dictionary<Vector2Int, Tile> Level = new Dictionary<Vector2Int, Tile>();

        public Vector3 GetPositionClosestTo(Vector3 position) =>
            new Vector3(Mathf.Round(position.x), 0.0f, Mathf.Round(position.z));

        public void AddTile(Vector3 position, Tile newTile, float height)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                DestroyImmediate(newTile.gameObject);
                return;
            }
            newTile.UpdateHeight(height);
            Level.Add(position.ToVector2IntXZ(), newTile);
        }

        public void RemoveTile(Vector3 position)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                Tile tile = Level[position.ToVector2IntXZ()];
                Level.Remove(position.ToVector2IntXZ());
                DestroyImmediate(tile.gameObject);
            }
        }

        public void ReplaceTile(Vector3 position, TileData brush, float height)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                Level[position.ToVector2IntXZ()].UpdateSprites(brush, height);
                Level[position.ToVector2IntXZ()].UpdateHeight(height);
            }
        }
    }
}