using Level;
using UnityEngine;

namespace Utilities
{
    public static class ExtensionMethods
    {
        public static Vector2Int ToVector2IntXZ(this Vector3 value) => new Vector2Int((int) value.x, (int) value.z);

        public static void SetLayerRecursively(this GameObject target, LayerMask layer)
        {
            target.layer = layer;
            foreach (Transform transform in target.transform) SetLayerRecursively(transform.gameObject, layer);
        }
    }
}