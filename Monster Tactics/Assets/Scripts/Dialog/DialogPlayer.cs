using System;
using System.Collections;
using System.Linq;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dialog
{
    public class DialogPlayer : MonoBehaviour
    {
        [SerializeField, InlineEditor]
        private DialogScript dialogScript;

        private DialogUI dialogUI;

        private CharacterMover mover;


        public void SetScript(DialogScript script)
        {
            dialogScript = Instantiate(script);
        }

        private bool goNext;
        private bool interactable;

        private void Start()
        {
            mover = FindObjectOfType<CharacterMover>();

            dialogScript = Instantiate(dialogScript);

            dialogUI = FindObjectOfType<DialogUI>();
            if (!dialogUI) Debug.LogWarning("No DialogUI found in scene!");
        }

        [Button]
        public IEnumerator NextMessage()
        {
            DialogEvent currentEvent = dialogScript.dialogEvents.Dequeue();

            switch (currentEvent.type)
            {
                case DialogEventType.Wait:
                    interactable = false;
                    yield return new WaitForSeconds(currentEvent.duration);
                    interactable = true;
                    break;
                case DialogEventType.Move:
                    if (currentEvent.leftCharacter)
                        dialogUI.SetLeftCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    else
                        dialogUI.SetRightCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    yield return StartCoroutine(MoveToTarget(currentEvent.CharacterData.name, currentEvent.target));
                    break;
                case DialogEventType.Attack:
                    if (currentEvent.leftCharacter)
                        dialogUI.SetLeftCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    else
                        dialogUI.SetRightCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);

                    break;
                case DialogEventType.Text:
                    if (currentEvent.leftCharacter)
                        dialogUI.SetLeftCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    else
                        dialogUI.SetRightCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    yield return StartCoroutine(dialogUI.SetDialogText(currentEvent.text, false));
                    break;
                case DialogEventType.Anim:
                    if (currentEvent.leftCharacter)
                        dialogUI.SetLeftCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    else
                        dialogUI.SetRightCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    break;
                case DialogEventType.Sprite:
                    if (currentEvent.leftCharacter)
                        dialogUI.SetLeftCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    else
                        dialogUI.SetRightCharacter(currentEvent.CharacterData.characterPortraitSprite,
                            currentEvent.CharacterData.name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Play()
        {
            StartCoroutine(PlayDialog());
        }

        public IEnumerator PlayDialog()
        {
            while (dialogScript.dialogEvents.Count > 0)
            {
                yield return StartCoroutine(NextMessage());
            }
        }

        private IEnumerator MoveToTarget(string name, Vector2Int target)
        {
            Character targetCharacter = FindObjectsOfType<Character>().First(x => x.Data.name == name);
            yield return mover.MoveToTarget(targetCharacter, target);
        }
    }
}