using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public struct MoveParams
    {
        public float duration;
        public string animatorTrigger;

        [HideLabel, SuffixLabel("Horizontal Movement", true)]
        public AnimationCurve horizontalMovement;

        [HideLabel, SuffixLabel("Vertical Movement", true)]
        public AnimationCurve verticalMovement;

        [HideLabel, SuffixLabel("Floor Bounce", true)]
        public AnimationCurve floorBounce;

        public MoveParams(float duration, AnimationCurve horizontalMovement,
            AnimationCurve verticalMovement, AnimationCurve floorBounce, string animatorTrigger)
        {
            this.duration = duration;
            this.horizontalMovement = horizontalMovement;
            this.verticalMovement = verticalMovement;
            this.floorBounce = floorBounce;
            this.animatorTrigger = animatorTrigger;
        }
    }
}