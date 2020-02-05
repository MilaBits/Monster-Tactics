using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Level.Editor
{
    [CustomPreview(typeof(TileData))]
    public class TileDataPreview : ObjectPreview
    {
        public override bool HasPreviewGUI() => true;

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            TileData data = (TileData) target;
            GUI.DrawTexture(r, data.top.texture, ScaleMode.ScaleToFit);
        }
    }
}