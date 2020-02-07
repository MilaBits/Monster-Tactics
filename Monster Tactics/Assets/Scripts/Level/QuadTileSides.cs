using UnityEngine;

namespace Level
{
    public class QuadTileSides : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer[] sides;

        public void SetMaterials(Material material, Color color)
        {
            for (int i = 0; i < sides.Length; i++)
            {
                sides[i].material.SetTexture("_BaseMap", material.GetTexture("_BaseMap"));
                sides[i].material.SetColor("_BaseColor", color);
            }
        }
    }
}