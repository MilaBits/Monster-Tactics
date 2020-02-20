using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField, InlineEditor]
        private CharacterData characterData = default;

        public CharacterData Data() => characterData;

        private SpriteRenderer spriteRenderer;
    }
}