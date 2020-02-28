using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class ActionWindow : MonoBehaviour
    {
        private TurnManager turnManager;
        private CharacterMover mover;

        [SerializeField]
        private Button MoveButton;

        private bool moved;

        private void Start()
        {
            turnManager = FindObjectOfType<TurnManager>();
            if (!turnManager) Debug.LogWarning("No Turn Manager present in scene!");
            mover = FindObjectOfType<CharacterMover>();
            if (!mover) Debug.LogWarning("No Character Mover present in scene!");
        }

        public void Move()
        {
            if (!moved && turnManager.CurrentCharacter.ActionPoints() >= 1)
            {
                mover.StartMove(ToggleWindow, turnManager.CurrentCharacter);
                turnManager.CurrentCharacter.LoseActionPoints(1);
                MoveButton.GetComponentInChildren<TextMeshProUGUI>().text = "Rush";
                moved = true;
            }
            else if (moved && turnManager.CurrentCharacter.ActionPoints() >= 2)
            {
                mover.StartMove(ToggleWindow, turnManager.CurrentCharacter);
                turnManager.CurrentCharacter.LoseActionPoints(2);
                MoveButton.interactable = false;
            }

            ToggleWindow(false, false);
        }

        private void ResetWindow()
        {
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
            gameObject.SetActive(toggle);
        }
    }
}