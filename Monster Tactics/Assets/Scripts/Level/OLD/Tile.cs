using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Level.OLD
{
    public class Tile : MonoBehaviour
    {
        [SerializeField, AssetList]
        private TileData tileData;

        [PropertyRange(0, 3)]
        public float height;

        [SerializeField]
        private List<TileSides> sideLayers;

        [SerializeField, FoldoutGroup("References")]
        private SpriteRenderer top;

        [SerializeField, FoldoutGroup("References")]
        private TileSides sidesPrefab;

        [Button]
        private void UpdateTile()
        {
            UpdateHeight(height);
            UpdateSprites(tileData, height);
        }


        public void UpdateSprites(TileData data, float height)
        {
            top.sprite = data.top;
            top.color = Color.white;
            foreach (TileSides tileSides in sideLayers)
            {
                tileSides.SetSprites(data.sides, Color.white);
            }
        }

        public void UpdateSprites(TileData data, Color color)
        {
            top.sprite = data.top;
            top.color = color;
            sideLayers.ForEach(x => x.SetSprites(data.sides, color));
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
            for (float posY = height;
                posY > -1;
                posY -= 1)
            {
                sideLayers.Add(Instantiate(
                    sidesPrefab,
                    transform.position + Vector3.up * posY,
                    Quaternion.Euler(-90, 0, 0),
                    transform));
            }
        }
    }
}