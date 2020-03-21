using Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public struct DialogEvent
    {
        [ToggleLeft, HorizontalGroup("timingSplit")]
        public bool waitUntilDone;

        [HorizontalGroup("timingSplit"), HideLabel, SuffixLabel("Seconds")]
        public float duration;

        [EnumToggleButtons, HideLabel]
        public DialogEventType type;

        // Character

        [BoxGroup("type", ShowLabel = false), LabelText("Char Data")]
        public CharacterData CharacterData;

        [BoxGroup("type", ShowLabel = false), LabelText("Left Char")]
        public bool leftCharacter;

        // Text

        [BoxGroup("text", ShowLabel = false), LabelText("$TextLabel")]
        public string text;

        [BoxGroup("text", ShowLabel = false), SuffixLabel("Seconds")]
        public float charInterval;

        // Move & Attack

        [BoxGroup("move & attack", ShowLabel = false)]
        public Vector2Int target;

        // Animation

        [BoxGroup("animation", ShowLabel = false), LabelText("Anim Trigger")]
        public string animationTrigger;

        // Sprite

        [BoxGroup("sprite", ShowLabel = false)]
        public string spriteTrigger;

        // inspector helpers
        private bool ShowTarget() => type == DialogEventType.Move || type == DialogEventType.Attack;
        private string TextLabel() => $"Text ({text.Length * charInterval}s)";
    }
}