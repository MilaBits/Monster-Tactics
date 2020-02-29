using System.Collections;
using Characters;
using Level;
using Sirenix.OdinInspector;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class ActionWindow : MonoBehaviour
    {
        [SerializeField, Required]
        private TurnManager turnManager;

        [SerializeField, Required]
        private CharacterMover mover;

        [SerializeField, Required]
        private ConfirmationDialog dialogPrefab;

        [SerializeField, Required]
        private Button MoveButton;

        private int moveCost = 1;

        private bool moved;

        private void ShowMove()
        {
            if (!moved && turnManager.CurrentCharacter.ActionPoints() >= moveCost)
            {
                mover.ShowPossible(turnManager.CurrentCharacter);
            }
            else if (moved && turnManager.CurrentCharacter.ActionPoints() >= moveCost)
            {
                mover.ShowPossible(turnManager.CurrentCharacter);
            }
        }

        public void ButtonMove() => StartCoroutine(Move());


        public IEnumerator Move()
        {
            ShowMove();

            ToggleWindow(false, false);

            QuadTile target = null;

            // Wait for click
            while (!target)
            {
                if (Input.GetButtonDown("Fire1")) target = mover.GetTarget();
                yield return null;
            }

            mover.StopUpdatingPath = true;

            ConfirmationDialog dialog = Instantiate(dialogPrefab, transform.parent);

            // Wait for dialog input
            while (dialog.Result == DialogResult.None)
            {
                yield return null;
            }


            if (dialog.Result == DialogResult.Yes)
            {
                Destroy(dialog.gameObject);
                CharacterData data = turnManager.CurrentCharacter.Data();
                yield return StartCoroutine(mover.Move(target.Path(), data.moveParams, data.jumpParams));
                moveCost++;

                if (moved) MoveButton.interactable = false;
                moved = true;
            }
            else
            {
                Destroy(dialog.gameObject);
            }

            mover.StopUpdatingPath = false;
            mover.Clear(PathfindingClear.Both);
            ToggleWindow(true, false);
        }

        private void ResetWindow()
        {
            moveCost = 1;
            MoveButton.interactable = true;
            moved = false;
            MoveButton.GetComponentInChildren<TextMeshProUGUI>().text = "Move";
        }

        public void Wait()
        {
            turnManager.NextTurn();
            ToggleWindow(false, false);
        }

        public void ToggleWindow(bool toggle, bool reset)
        {
            if (reset) ResetWindow();
            transform.GetChild(0).gameObject.SetActive(toggle);

            if (moved) MoveButton.GetComponentInChildren<TextMeshProUGUI>().text = "Rush";

            if (toggle && moveCost > turnManager.CurrentCharacter.ActionPoints()) MoveButton.interactable = false;
        }
    }
}