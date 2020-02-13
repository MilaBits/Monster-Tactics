using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public static Vector3 GetPositionClosestTo(Vector3 position) =>
            new Vector3(Mathf.Round(position.x), 0.0f, Mathf.Round(position.z));

        public List<QuadTile> Neighbors(Vector2Int position)
        {
            List<QuadTile> result = new List<QuadTile>();
            QuadTile value;
            if (Level.TryGetValue(position + Vector2Int.left, out value)) result.Add(value);
            if (Level.TryGetValue(position + Vector2Int.up, out value)) result.Add(value);
            if (Level.TryGetValue(position + Vector2Int.right, out value)) result.Add(value);
            if (Level.TryGetValue(position + Vector2Int.down, out value)) result.Add(value);
            return result;
        }

        [SerializeField, HideInInspector]
        private QuadTile tilePrefab = default;

        public QuadTile GetTile(int x, int y) => Level[new Vector2Int(x, y)];
        public QuadTile GetTile(Vector2Int val) => Level[val];

        public List<QuadTile> GetTilesInRange(QuadTile start, int range, bool useRoughness)
        {
            ResetPathfindingData();

            List<QuadTile> possibleTiles = new List<QuadTile>();
            Queue<QuadTile> frontier = new Queue<QuadTile>();
            frontier.Enqueue(start);
            start.pathFindingData.visited = true;

            while (frontier.Count > 0)
            {
                frontier = new Queue<QuadTile>(frontier.OrderBy(x => x.GetChainValue(0)));
                QuadTile current = frontier.Dequeue();


                foreach (var neighbor in Neighbors(current.transform.position.ToVector2IntXZ()))
                {
                    if (neighbor.pathFindingData.visited) continue;

                    frontier.Enqueue(neighbor);
                    neighbor.pathFindingData.visited = true;
                    neighbor.pathFindingData.cameFrom = current;

                    Debug.DrawLine(neighbor.transform.position, current.transform.position, Color.blue, 2f);
                    
                    possibleTiles.Add(neighbor);
                }
            }

            return possibleTiles.Where(x => x.GetChainValue(-start.pathFindingData.cost) <= range && x.GetChainValue(-start.pathFindingData.cost) > 0).ToList();
        }

        private void ResetPathfindingData()
        {
            foreach (KeyValuePair<Vector2Int, QuadTile> keyValuePair in Level)
            {
                keyValuePair.Value.ToggleViableMarker(false);
                keyValuePair.Value.pathFindingData =
                    new QuadTile.PathfindingData(false, keyValuePair.Value.Data().difficulty);
            }
        }

        public void AddTile(Vector3 position, QuadTileData brush, float height)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                ReplaceTile(position, brush, height);
                return;
            }

            QuadTile newTile = Instantiate(tilePrefab, position, tilePrefab.transform.rotation, transform);
            newTile.Init(brush, height);
            newTile.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ignore Editor"));

            Level.Add(position.ToVector2IntXZ(), newTile);
        }

        public void DeleteTile(Vector3 position)
        {
            if (!Level.ContainsKey(position.ToVector2IntXZ())) return;

            QuadTile tile = Level[position.ToVector2IntXZ()];
            Level.Remove(position.ToVector2IntXZ());
            DestroyImmediate(tile.gameObject);
        }

        private void ReplaceTile(Vector3 position, QuadTileData brush, float height)
        {
            if (Level.ContainsKey(position.ToVector2IntXZ()))
            {
                Level[position.ToVector2IntXZ()].Init(brush, height);
            }
        }
    }
}