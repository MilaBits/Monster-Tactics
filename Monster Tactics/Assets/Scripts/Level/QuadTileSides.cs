using UnityEngine;

namespace Level
{
    public class QuadTileSides : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer[] sides = default;

        public void SetMaterials(Material material)
        {
            for (int i = 0; i < sides.Length; i++)
            {
                sides[i].material = material;
            }
        }

        public void SetSelectionMaterial(Material material, Color color)
        {
            for (int i = 0; i < sides.Length; i++)
            {
                sides[i].sharedMaterial.SetTexture("_BaseMap", material.GetTexture("_BaseMap"));
                sides[i].sharedMaterial.SetColor("_BaseColor", color);
            }
        }
    }
}