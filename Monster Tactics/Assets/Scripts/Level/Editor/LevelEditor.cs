using System;
using System.Security.Policy;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Level.Editor
{
    public class LevelEditor : OdinEditorWindow
    {
        [FoldoutGroup("Create New Brush"), SerializeField, AssetList(Path = "/Sprites/Tiles/"),
         PreviewField(ObjectFieldAlignment.Center)]
        private Sprite top = default;

        [FoldoutGroup("Create New Brush"), SerializeField, AssetList(Path = "/Sprites/Tiles/"),
         PreviewField(ObjectFieldAlignment.Center)]
        private Sprite side = default;

        [FoldoutGroup("Create New Brush"), SerializeField]
        private string brushName = default;

        [SerializeField, AssetList, InlineEditor]
        private TileData brush = default;

        [HorizontalGroup("EditSplit"), BoxGroup("EditSplit/Mode")]
        [SerializeField, EnumToggleButtons, HideLabel]
        private LevelEditMode editMode = LevelEditMode.None;

        private Tile tilePrefab = default;
        private Tile _selectionTile = default;

        [Button, BoxGroup("EditSplit/Height"), LabelText("-")]
        private void DecreaseHeight() => height = Mathf.Clamp(height - .5f, 0, 3);

        [SerializeField, BoxGroup("EditSplit/Height"), OnValueChanged("RoundHalf"), HideLabel]
        private float height = 0;

        [Button, BoxGroup("EditSplit/Height"), LabelText("+")]
        private void IncreaseHeight() => height = Mathf.Clamp(height + .5f, 0, 3);

        private void RoundHalf() => height = (float) Math.Round(height * 2, MidpointRounding.AwayFromZero) / 2;

        private bool AllowAdd() => top != null && side != null && !brushName.IsNullOrWhitespace();

        private readonly Color ADD_COLOR = new Color(1, 1, 0, .5f);
        private readonly Color REP_COLOR = new Color(0, 0, 1, .5f);
        private readonly Color REM_COLOR = new Color(1, 0, 0, .5f);
        private readonly Color INV_COLOR = new Color(1, 1, 1, 0);

        private const KeyCode ADD_KEY = KeyCode.Z;
        private const KeyCode REP_KEY = KeyCode.X;
        private const KeyCode REM_KEY = KeyCode.C;
        private const KeyCode NON_KEY = KeyCode.V;


        [FoldoutGroup("Create New Brush"), Button, LabelText("Add Brush"), EnableIf("AllowAdd")]
        private void CreateTileBrush()
        {
            TileData data = CreateInstance<TileData>();
            data.top = top;
            data.sides = side;

            AssetDatabase.CreateAsset(data, $"Assets/Tiles/{brushName}.asset");
            AssetDatabase.SaveAssets();
        }


        protected override void OnEnable()
        {
            SceneView.duringSceneGui -= OnSceneGui;
            SceneView.duringSceneGui += OnSceneGui;

            tilePrefab = AssetDatabase.LoadAssetAtPath<Tile>("Assets/Prefabs/Tile.prefab");
            brush = AssetDatabase.LoadAssetAtPath<TileData>("Assets/tiles/GrassFlat.asset");

            _selectionTile = Instantiate(tilePrefab);
            _selectionTile.name = "Level Editor Selection";

            base.OnEnable();
        }

        protected void OnDisable()
        {
            DestroyImmediate(_selectionTile.gameObject);
            SceneView.duringSceneGui -= OnSceneGui;
        }

        [MenuItem("Monster Tactics/Level Editor")]
        public static void OpenWindow() => GetWindow<LevelEditor>().Show();

        private void OnSceneGui(SceneView sv)
        {
            DrawControls();

            if (editMode == LevelEditMode.None) return;

            TileMap tileMap = FindObjectOfType<TileMap>();

            Event e = Event.current;
            // Convert mouse position to world position by finding point where y = 0.
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Vector3 pos = ray.origin - (ray.origin.y / ray.direction.y) * ray.direction;
            Vector3 snappedPosition = tileMap.GetPositionClosestTo(pos);

            // Set ghost tile's position
            _selectionTile.transform.position = snappedPosition;
            _selectionTile.UpdateHeight(height);

            switch (editMode)
            {
                case LevelEditMode.Add:
                    _selectionTile.UpdateSprites(brush, ADD_COLOR);
                    break;
                case LevelEditMode.Remove:
                    _selectionTile.UpdateSprites(brush, REM_COLOR);
                    break;
                case LevelEditMode.Replace:
                    _selectionTile.UpdateSprites(brush, REP_COLOR);
                    break;
                case LevelEditMode.None:
                    _selectionTile.UpdateSprites(brush, INV_COLOR);
                    break;
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            if (e.GetTypeForControl(controlId) == EventType.ScrollWheel && e.control)
            {
                if (e.delta.y > 0) DecreaseHeight();
                else if (e.delta.y < 0) IncreaseHeight();
                e.Use();
            }

            if (e.GetTypeForControl(controlId) == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    case NON_KEY:
                        editMode = LevelEditMode.None;
                        break;
                    case ADD_KEY:
                        editMode = LevelEditMode.Add;
                        break;
                    case REP_KEY:
                        editMode = LevelEditMode.Replace;
                        break;
                    case REM_KEY:
                        editMode = LevelEditMode.Remove;
                        break;
                }
                Repaint();
            }

            if (e.GetTypeForControl(controlId) == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    switch (editMode)
                    {
                        case LevelEditMode.Add:
                            Tile newTile = Instantiate(tilePrefab, snappedPosition, tilePrefab.transform.rotation,
                                tileMap.transform);
                            newTile.UpdateSprites(brush, height);
                            tileMap.AddTile(snappedPosition, newTile, height);
                            break;
                        case LevelEditMode.Remove:
                            tileMap.RemoveTile(snappedPosition);
                            break;
                        case LevelEditMode.Replace:
                            tileMap.ReplaceTile(snappedPosition, brush, height);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    e.Use();
                }
            }
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
            GUILayout.Label("Replace");
            GUILayout.Label("Remove");
            GUILayout.Space(5);
            GUILayout.Label("Height");
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);
            GUILayout.Label("");
            GUILayout.Space(5);
            GUILayout.Label(ADD_KEY.ToString());
            GUILayout.Label(REP_KEY.ToString());
            GUILayout.Label(REM_KEY.ToString());
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