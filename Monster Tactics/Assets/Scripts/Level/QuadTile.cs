﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Level
{
    public class QuadTile : MonoBehaviour, IEquatable<QuadTile>
    {
        [SerializeField, AssetList]
        private QuadTileData tileData = default;

        [PropertyRange(0, 3)]
        public float height;

        [SerializeField]
        private List<QuadTileSides> sideLayers = default;

        [SerializeField, FoldoutGroup("References")]
        private MeshRenderer top = default;

        [SerializeField, FoldoutGroup("References")]
        private QuadTileSides sidesPrefab = default;

        [SerializeField, FoldoutGroup("References")]
        private GameObject viableMarker = default;

        public PathfindingData pathFindingData;

        public Vector3 PositionWithHeight() => transform.position + Vector3.up * height;

        public struct PathfindingData
        {
            public bool visited;
            public int cost;
            public QuadTile cameFrom;

            public PathfindingData(bool visited, int cost)
            {
                this.visited = visited;
                this.cost = cost;
                cameFrom = null;
            }
        }

        public void ToggleViableMarker(bool value)
        {
            viableMarker.layer = LayerMask.GetMask("Viable Marker");
            viableMarker.SetActive(value);
        }

        public int GetChainValue(int value, bool useRoughness)
        {
            value = useRoughness ? pathFindingData.cost : 1;

            if (pathFindingData.cameFrom)
            {
                value += pathFindingData.cameFrom.GetChainValue(value, useRoughness);
            }

            return value;
        }

        public void UpdateDebugText(string text) => GetComponentInChildren<TextMeshPro>().text = text;

        [Button]
        private void RefreshTile()
        {
            UpdateHeight(height);
            UpdateMaterials(tileData);
        }

        public QuadTileData Data() => tileData;

        private void UpdateMaterials(QuadTileData data)
        {
            tileData = data;
            top.sharedMaterial = data.top;
            sideLayers.ForEach(x => x.SetMaterials(data.sides));
        }

        public void UpdateSelectionMaterials(QuadTileData data, Color color)
        {
            top.sharedMaterial.SetTexture("_BaseMap", data.top.GetTexture("_BaseMap"));
            top.sharedMaterial.SetColor("_BaseColor", color);
            sideLayers.ForEach(x => x.SetSelectionMaterial(data.sides, color));
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

        public bool Equals(QuadTile other) => transform.position == other.transform.position;

        public List<QuadTile> ChainToList()
        {
            List<QuadTile> tiles = new List<QuadTile>();

            QuadTile current = this;

            while (current.pathFindingData.cameFrom != null)
            {
                tiles.Add(current);
                current = current.pathFindingData.cameFrom;
            }

            return tiles;
        }
    }
}