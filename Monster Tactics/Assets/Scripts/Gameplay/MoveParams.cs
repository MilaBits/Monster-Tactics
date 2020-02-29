using System;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public struct MoveParams
    {
        public AnimationCurve horizontalMovement;
        public AnimationCurve verticalMovement;
        public AnimationCurve floorBounce;

        public float Duration => horizontalMovement.keys.Last().time;

        public MoveParams(float duration, AnimationCurve horizontalMovement,
            AnimationCurve verticalMovement, AnimationCurve floorBounce)
        {
            this.horizontalMovement = horizontalMovement;
            this.verticalMovement = verticalMovement;
            this.floorBounce = floorBounce;
        }
    }
}