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
    }
}