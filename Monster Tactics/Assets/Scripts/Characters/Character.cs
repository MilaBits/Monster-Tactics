using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField, InlineEditor]
        private CharacterData characterData = default;

        private int Health;

        public CharacterData Data() => characterData;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = Data().animatorController;

            Health = Data().MaxHealth;
        }

        public void FlipCharacter(bool flip)
        {
            spriteRenderer.flipX = flip;
        }

        public void ChangeAnimation(string animation) => animator.SetTrigger(animation);
    }
}