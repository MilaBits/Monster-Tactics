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
        private TurnManager turnManager = default;

        [SerializeField, Required]
        private CharacterMover mover = default;

        [SerializeField, Required]
        private CharacterAttacker attacker = default;

        [SerializeField, Required]
        private ConfirmationDialog dialogPrefab = default;

        [SerializeField, Required]
        private Button MoveButton = default;

        [SerializeField, Required]
        private Button AttackButton = default;

        private int attackCost = 1;

        private int moveCost = 1;
        private bool moved;

        public void ButtonAttack() => StartCoroutine(Attack());

        public IEnumerator Attack()
        {
            attacker.ShowPossible(turnManager.CurrentCharacter);

            ToggleWindow(false, false);

            QuadTile target = null;

            while (!target)
            {
                if (Input.GetButtonDown("Fire1")) target = QuadTileMap.GetTarget(LayerMask.GetMask("Viable Marker"));
                yield return null;
            }

            ConfirmationDialog dialog = Instantiate(dialogPrefab, transform.parent);

            // Wait for dialog input
            while (dialog.Result == DialogResult.None)
            {
                yield return null;
            }

            if (dialog.Result == DialogResult.Yes)
            {
                Destroy(dialog.gameObject);

                CharacterData data = turnManager.CurrentCharacter.Data;
                attacker.Attack(target);
                turnManager.CurrentCharacter.LoseActionPoints(attackCost);
                attackCost++;

                if (moved) MoveButton.interactable = false;
                moved = true;
            }
            else
            {
                Destroy(dialog.gameObject);
            }

            attacker.Clear();
            ToggleWindow(true, false);
        }

        private void ShowMove()
        {
            if (!moved && turnManager.CurrentCharacter.ActionPoints() >= moveCost)
                mover.ShowPossible(turnManager.CurrentCharacter);
            else if (moved && turnManager.CurrentCharacter.ActionPoints() >= moveCost)
                mover.ShowPossible(turnManager.CurrentCharacter);
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
                if (Input.GetButtonDown("Fire1")) target = QuadTileMap.GetTarget(LayerMask.GetMask("Viable Marker"));
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
                CharacterData data = turnManager.CurrentCharacter.Data;
                MoveParams moveParams = moved ? data.rushParams : data.moveParams;
                yield return StartCoroutine(mover.Move(turnManager.CurrentCharacter, target.Path(), moveParams,
                    data.jumpParams));
                turnManager.CurrentCharacter.LoseActionPoints(moveCost);
                moveCost++;

                if (moved) MoveButton.interactable = false;
                moved = true;
            }
            else
            {
                Destroy(dialog.gameObject);
            }

            mover.Clear(PathfindingClear.Both);
            ToggleWindow(true, false);
        }

        private void ResetWindow()
        {
            attackCost = 1;
            AttackButton.interactable = true;

            moveCost = 1;
            MoveButton.interactable = true;
            moved = false;
            MoveButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Move";
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

            if (moved) MoveButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Rush";

            if (toggle && moveCost > turnManager.CurrentCharacter.ActionPoints()) MoveButton.interactable = false;
            if (toggle && attackCost > turnManager.CurrentCharacter.ActionPoints()) AttackButton.interactable = false;
        }
    }
}