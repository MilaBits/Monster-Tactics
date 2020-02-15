using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "New Character Data", menuName = "Monster Tactics/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [Header("Stats")]
        public int damage;

        public int defense;
        public int move;
        [Range(0,10), OnValueChanged("RoundHalf")]
        public float stepLayerLimit;
        public bool useRoughness;
        
        private void RoundHalf() => stepLayerLimit = (float) Math.Round(stepLayerLimit * 2, MidpointRounding.AwayFromZero) / 2;
    }
}