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

        public CharacterData Data() => characterData;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = Data().animatorController;

            health = Data().MaxHealth;
        }

        private void Update()
        {
            spriteRenderer.transform.rotation = Camera.main.transform.rotation;
        }

        public void FlipCharacter(bool flip) => spriteRenderer.flipX = flip;
        public void ChangeAnimation(string animation) => animator.SetTrigger(animation);
        public void RefillActionPoints() => actionPoints = Data().MaxActionPoints;
        public void LoseActionPoints(int amount) => actionPoints -= amount;
        public int ActionPoints() => actionPoints;
    }
}