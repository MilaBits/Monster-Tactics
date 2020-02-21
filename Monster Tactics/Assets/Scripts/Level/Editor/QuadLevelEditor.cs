using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Utilities;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;

namespace Level.Editor
{
    public class QuadLevelEditor : OdinEditorWindow
    {
        [FoldoutGroup("Create New Brush"), SerializeField, AssetList(Path = "/Textures/Tiles/"),
         PreviewField(ObjectFieldAlignment.Center)]
        private Texture top = default;

        [FoldoutGroup("Create New Brush"), SerializeField, AssetList(Path = "/Textures/Tiles/"),
         PreviewField(ObjectFieldAlignment.Center)]
        private Texture side = default;

        [FoldoutGroup("Create New Brush"), SerializeField]
        private string brushName = default;

        [SerializeField, AssetList]
        private QuadTileData brush = default;

        [FoldoutGroup("$EditModeString")]
        [SerializeField, EnumToggleButtons, HideLabel]
        private LevelEditMode editMode = LevelEditMode.None;

        private LevelEditMode oldMode;

        private QuadTile tilePrefab = default;
        private QuadTile selectionPrefab = default;
        private QuadTile _selectionTile = default;

        private const float maxHeight = 5f;
        private const float minHeight = .5f;

        [Button, FoldoutGroup("$HeightString"), LabelText("-")]
        private void DecreaseHeight() => height = Mathf.Clamp(height - minHeight, 0, maxHeight);

        [SerializeField, FoldoutGroup("$HeightString"), OnValueChanged("RoundHalf"), HideLabel]
        private float height = 0;

        [Button, FoldoutGroup("$HeightString"), LabelText("+")]
        private void IncreaseHeight() => height = Mathf.Clamp(height + minHeight, 0, maxHeight);

        private string EditModeString() => $"Mode - {editMode.ToString()}";
        private string HeightString() => $"Height - {height}";
        private void RoundHalf() => height = (float) Math.Round(height * 2, MidpointRounding.AwayFromZero) / 2;
        private bool AllowAdd() => top != null && side != null && !brushName.IsNullOrWhitespace();

        private readonly Color ADD_COLOR = new Color(0, 1, 0, .5f);
        private readonly Color REM_COLOR = new Color(1, 0, 0, .5f);
        private readonly Color INV_COLOR = new Color(1, 1, 1, 0);

        private const KeyCode ADD_KEY = KeyCode.A;
        private const KeyCode REM_KEY = KeyCode.D;
        private const KeyCode NON_KEY = KeyCode.F;

        private LayerMask ignoreLayer;
        private LayerMask defaultLayer;

        private QuadTileMap tileMap;

        private Vector3 lastSnappedPosition;

        [FoldoutGroup("Save & Load"), Button]
        private void SaveMap()
        {
            SavedLevelData levelData = CreateInstance<SavedLevelData>();
            foreach (KeyValuePair<Vector2Int, QuadTile> tile in tileMap.GetTiles())
            {
                levelData.Tiles.Add(tile.Key,
                    new SavedLevelData.SavedTileData {Data = tile.Value.Data(), Height = tile.Value.height});
            }

            string path = "Assets" + EditorUtility
                              .SaveFilePanel("Save Level", "Assets/Levels/", "New Level.asset", "asset")
                              .Substring(Application.dataPath.Length);

            SavedLevelData data = AssetDatabase.LoadAssetAtPath<SavedLevelData>(path);

            AssetDatabase.CreateAsset(levelData, path);
        }

        [FoldoutGroup("Save & Load"), Button]
        private void LoadMap() => tileMap.LoadMap();

        [FoldoutGroup("Save & Load"), Button]
        private void ClearMap() => tileMap.Clear();

        protected override void OnEnable()
        {
            SceneView.duringSceneGui -= OnSceneGui;
            SceneView.duringSceneGui += OnSceneGui;

            ignoreLayer = LayerMask.NameToLayer("Ignore Editor");
            defaultLayer = LayerMask.NameToLayer("Default");

            tilePrefab = AssetDatabase.LoadAssetAtPath<QuadTile>("Assets/Prefabs/QuadTile.prefab");
            selectionPrefab = AssetDatabase.LoadAssetAtPath<QuadTile>("Assets/Prefabs/Editor/SelectionTile.prefab");
            brush = AssetDatabase.LoadAssetAtPath<QuadTileData>("Assets/tiles/Mat/GrassLight.asset");

            _selectionTile = Instantiate(selectionPrefab);

            _selectionTile.name = "SelectionMarker";

            base.OnEnable();
        }

        protected void OnDisable()
        {
            DestroyImmediate(_selectionTile.gameObject);
            SceneView.duringSceneGui -= OnSceneGui;

            ChangeTilemapLayer(defaultLayer);
        }

        private void ChangeTilemapLayer(LayerMask layer)
        {
            foreach (KeyValuePair<Vector2Int, QuadTile> quadTile in tileMap.GetTiles())
            {
                quadTile.Value.gameObject.SetLayerRecursively(layer);
            }
        }

        [MenuItem("Monster Tactics/Quad Level Editor")]
        public static void OpenWindow() => GetWindow<QuadLevelEditor>().Show();

        private void OnSceneGui(SceneView sv)
        {
            DrawControls();

            tileMap = FindObjectOfType<QuadTileMap>();

            _selectionTile.gameObject.SetActive(editMode != LevelEditMode.None);

            Event e = Event.current;
            // Convert mouse position to world position by finding point where y = 0.
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Vector3 pos = ray.origin - (ray.origin.y / ray.direction.y) * ray.direction;
            Vector3 snappedPosition = QuadTileMap.GetPositionClosestTo(pos);

            _selectionTile.transform.position = snappedPosition;

            _selectionTile.UpdateHeight(height);
            ModeChanged();

            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            if (e.GetTypeForControl(controlId) == EventType.ScrollWheel && e.control)
            {
                if (e.delta.y > 0) DecreaseHeight();
                else if (e.delta.y < 0) IncreaseHeight();
                e.Use();
            }

            if (e.GetTypeForControl(controlId) == EventType.KeyDown && e.control)
            {
                switch (e.keyCode)
                {
                    case NON_KEY:
                        editMode = LevelEditMode.None;
                        break;
                    case ADD_KEY:
                        editMode = LevelEditMode.Add;
                        break;
                    case REM_KEY:
                        editMode = LevelEditMode.Delete;
                        break;
                }

                e.Use();
                Repaint();
            }

            if (e.GetTypeForControl(controlId) == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    switch (editMode)
                    {
                        case LevelEditMode.Add:
                            tileMap.AddTile(snappedPosition, brush, height);
                            break;
                        case LevelEditMode.Delete:
                            tileMap.DeleteTile(snappedPosition);
                            break;
                        case LevelEditMode.None:
                            return;
                    }
                }
            }

            lastSnappedPosition = snappedPosition;
        }

        private void ModeChanged()
        {
            if (oldMode != editMode)
            {
                switch (editMode)
                {
                    case LevelEditMode.Add:
                        _selectionTile.UpdateSelectionMaterials(brush, ADD_COLOR);
                        ChangeTilemapLayer(ignoreLayer);
                        break;
                    case LevelEditMode.Delete:
                        _selectionTile.UpdateSelectionMaterials(brush, REM_COLOR);
                        ChangeTilemapLayer(ignoreLayer);
                        break;
                    case LevelEditMode.None:
                        _selectionTile.UpdateSelectionMaterials(brush, INV_COLOR);
                        ChangeTilemapLayer(defaultLayer);
                        break;
                }
            }

            oldMode = editMode;
        }

        [FoldoutGroup("Create New Brush"), Button, LabelText("Add Brush"), EnableIf("AllowAdd")]
        private void CreateTileBrush()
        {
            QuadTileData data = CreateInstance<QuadTileData>();

            Material topMat = AssetDatabase.LoadAssetAtPath<Material>($"Assets/Materials/Tiles/{top.name}.mat");
            if (topMat)
            {
                data.top = topMat;
            }
            else
            {
                topMat = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
                topMat.SetTexture("_BaseMap", top);
                topMat.SetColor("_BaseColor", Color.white);
                AssetDatabase.CreateAsset(topMat, $"Assets/Materials/Tiles/{top.name}.mat");
                AssetDatabase.SaveAssets();
                data.top = AssetDatabase.LoadAssetAtPath<Material>($"Assets/Materials/Tiles/{top.name}.mat");
            }

            Material sideMat = AssetDatabase.LoadAssetAtPath<Material>($"Assets/Materials/Tiles/{side.name}.mat");
            if (sideMat)
            {
                data.sides = sideMat;
            }
            else
            {
                sideMat = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
                sideMat.SetTexture("_BaseMap", side);
                sideMat.SetColor("_BaseColor", Color.white);
                AssetDatabase.CreateAsset(sideMat, $"Assets/Materials/Tiles/{side.name}.mat");
                AssetDatabase.SaveAssets();
                data.sides = AssetDatabase.LoadAssetAtPath<Material>($"Assets/Materials/Tiles/{side.name}.mat");
            }

            AssetDatabase.CreateAsset(data, $"Assets/Tiles/Mat/{brushName}.asset");
            AssetDatabase.SaveAssets();
            GUIHelper.RequestRepaint();
        }

        private void DrawControls()
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(20, 20, 150, 200));
            GUI.backgroundColor = new Color(.5f, .5f, .5f, .5f);
            var rect = EditorGUILayout.BeginHorizontal();
            GUI.Box(rect, GUIContent.none);
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginVertical();
            GUI.color = Color.white;
            GUILayout.Space(5);
            GUILayout.Label("Hotkeys");
            GUILayout.Space(5);
            GUILayout.Label("Add");
            GUILayout.Label("Delete");
            GUILayout.Space(5);
            GUILayout.Label("Height");
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.Label("");
            GUILayout.Space(5);
            GUILayout.Label("Ctrl + " + ADD_KEY);
            GUILayout.Label("Ctrl + " + REM_KEY);
            GUILayout.Space(5);
            GUILayout.Label("Ctrl + Scroll");
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
            Handles.EndGUI();
        }
    }
}