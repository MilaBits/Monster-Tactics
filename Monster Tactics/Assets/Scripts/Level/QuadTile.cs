using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Level
{
    public class QuadTile : MonoBehaviour
    {
        [SerializeField, AssetList]
        private QuadTileData tileData;

        [PropertyRange(0, 3)]
        public float height;

        [SerializeField]
        private List<QuadTileSides> sideLayers;

        [SerializeField, FoldoutGroup("References")]
        private MeshRenderer top;

        [SerializeField, FoldoutGroup("References")]
        private QuadTileSides sidesPrefab;

        [Button]
        private void RefreshTile()
        {
            UpdateHeight(height);
            UpdateMaterials(tileData);
        }

        private void UpdateMaterials(QuadTileData data)
        {
            tileData = data;
            top.material = data.top;
            top.sharedMaterial.color = Color.white;
            sideLayers.ForEach(x => x.SetMaterials(data.sides, Color.white));
        }

        public void UpdateMaterials(QuadTileData data, Color color)
        {
            top.material = data.top;
            top.sharedMaterial.color = color;
            sideLayers.ForEach(x => x.SetMaterials(data.sides, color));
        }

        public void UpdateHeight(float value)
        {
            height = value;
            top.transform.position = transform.position + Vector3.up * height;
            for (int i = sideLayers.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(sideLayers[i].gameObject);
            }

            sideLayers.Clear();
            for (float posY = height; posY > -1; posY -= 1)
            {
                sideLayers.Add(Instantiate(
                    sidesPrefab,
                    transform.position + Vector3.up * posY,
                    Quaternion.identity,
                    transform));
            }
        }

        public void Init(QuadTileData brush, float height)
        {
            tileData = brush;
            this.height = height;
            RefreshTile();
        }
    }
}