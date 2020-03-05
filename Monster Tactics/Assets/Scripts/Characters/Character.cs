using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [SerializeField, InlineEditor]
        private CharacterData characterData = default;

        private int health;
        private int actionPoints;

        public CharacterData Data => characterData;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private Vector3 oldCameraPos;

        private bool flippedByDefault;

        private string lastAnimation;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            flippedByDefault = spriteRenderer.flipX;

            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = Data.animatorController;

            health = Data.MaxHealth;
        }

        private void Update()
        {
            spriteRenderer.transform.rotation = Camera.main.transform.rotation;
        }

        public void FlipCharacter(bool flip) => spriteRenderer.flipX = flip;
        public void ResetFlip() => spriteRenderer.flipX = flippedByDefault;

        public void ChangeAnimation(string animation)
        {
            if (lastAnimation == animation) return;
            animator.SetTrigger(animation);
            lastAnimation = animation;
        }

        public void RefillActionPoints() => actionPoints = Data.MaxActionPoints;
        public void LoseActionPoints(int amount) => actionPoints -= amount;
        public int ActionPoints() => actionPoints;
    }
}