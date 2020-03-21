using System.Collections.Generic;
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

        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= list.Count) || (0 > newIndex) ||
                (newIndex >= list.Count)) return;
            // local variables
            var i = 0;
            T tmp = list[oldIndex];
            for (i = oldIndex + 1; i < newIndex; i++)
            {
                list[i] = list[i - 1];
            }

            list[newIndex] = tmp;
        }

        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }
}