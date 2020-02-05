using UnityEngine;

namespace Utilities
{
    public static class Extensions
    {
        public static Vector2Int ToVector2IntXZ(this Vector3 value) => new Vector2Int((int) value.x, (int) value.z);
    }
}