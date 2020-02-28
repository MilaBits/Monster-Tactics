using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "New Character Data", menuName = "Monster Tactics/Character Data")]
    public class CharacterData : SerializedScriptableObject
    {
        [Header("Stats")]
        public int MaxHealth;

        public int MaxActionPoints;

        public int Attack;
        public int defense;
        public int move;

        [Space]
        public int startPriority;

        [Range(0, 10), OnValueChanged("RoundHalf")]
        public float stepLayerLimit = .5f;

        public bool useRoughness = true;

        [Space]
        public AnimatorController animatorController;

        private void RoundHalf() =>
            stepLayerLimit = (float) Math.Round(stepLayerLimit * 2, MidpointRounding.AwayFromZero) / 2;
    }
}