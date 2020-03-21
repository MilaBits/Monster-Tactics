using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Dialog
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField, BoxGroup("left")]
        private Image leftCharacterSprite = default;

        [SerializeField, BoxGroup("left")]
        private TMP_Text leftCharacterName = default;

        [SerializeField, BoxGroup("right")]
        private Image rightCharacterSprite = default;

        [SerializeField, BoxGroup("right")]
        private TMP_Text rightCharacterName = default;

        [SerializeField]
        private TMP_Text dialogText = default;

        public void SetLeftCharacter(Sprite portrait, string characterName)
        {
            leftCharacterSprite.sprite = portrait;
            leftCharacterName.text = characterName;
        }

        public void SetRightCharacter(Sprite portrait, string characterName)
        {
            rightCharacterSprite.sprite = portrait;
            rightCharacterName.text = characterName;
        }

        public IEnumerator SetDialogText(string text, bool append)
        {
            dialogText.text = append ? dialogText.text + text : text;
            yield return StartCoroutine(RevealText(.07f));
        }

        private IEnumerator RevealText(float interval)
        {
            var originalString = dialogText.text;
            dialogText.text = "";

            var numCharsRevealed = 0;
            while (numCharsRevealed < originalString.Length)
            {
                while (originalString[numCharsRevealed] == ' ')
                    ++numCharsRevealed;

                ++numCharsRevealed;

                dialogText.text = originalString.Substring(0, numCharsRevealed);

                yield return new WaitForSeconds(interval);
            }
        }
    }
}