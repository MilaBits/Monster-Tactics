using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class StatsBox : MonoBehaviour
    {
        [SerializeField]
        public LabelValueField HealthField;

        [SerializeField]
        public LabelValueField DefenseField;

        [SerializeField]
        public LabelValueField AttackField;

        [SerializeField]
        public LabelValueField MoveField;

        public void SetFields(string health, string defense, string attack, string move)
        {
        }

        public void ToggleWindow(bool toggle)
        {
        }
    }
}