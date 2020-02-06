using UnityEngine;

namespace Level.OLD
{
    public class TileSides : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] sides;

        public void SetSprites(Sprite sprite, Color color)
        {
            for (int i = 0; i < sides.Length; i++)
            {
                sides[i].sprite = sprite;
                sides[i].color = color;
            }
        }
    }
}