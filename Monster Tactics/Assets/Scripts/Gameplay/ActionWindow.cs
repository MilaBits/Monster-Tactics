using System;
using Characters;
using UnityEngine;

namespace Gameplay
{
    public class ActionWindow : MonoBehaviour
    {
        private TurnManager turnManager;
        private CharacterMover mover;

        private void Start()
        {
            turnManager = FindObjectOfType<TurnManager>();
            if (!turnManager) Debug.LogWarning("No Turn Manager present in scene!");
            mover = FindObjectOfType<CharacterMover>();
            if (!mover) Debug.LogWarning("No Character Mover present in scene!");
        }

        public void Move()
        {
            mover.StartMove(ToggleWindow, turnManager.CurrentCharacter);
            ToggleWindow(false);
        }

        public void Wait()
        {
            turnManager.NextTurn();
            ToggleWindow(false);
        }

        public void ToggleWindow(bool toggle)
        {
            gameObject.SetActive(toggle);
        }
    }
}