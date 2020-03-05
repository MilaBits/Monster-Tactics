using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Queue<Character> Characters;

    private Character ActiveCharacter;

    private CameraController cameraController;

    private GameUIManager gameUiManager;

    public Character CurrentCharacter => ActiveCharacter;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        if (!cameraController) Debug.LogWarning("No Camera Controller present in scene!");

        gameUiManager = FindObjectOfType<GameUIManager>();
        if (!gameUiManager) Debug.LogWarning("No Game UI Manager present in scene!");

        Characters = new Queue<Character>(FindObjectsOfType<Character>().OrderBy(x => x.Data.startPriority));

        StartCoroutine(StartTurn());
    }

    public void NextTurn()
    {
        Characters.Enqueue(ActiveCharacter);
        StartCoroutine(StartTurn());
    }

    private IEnumerator StartTurn()
    {
        ActiveCharacter = Characters.Dequeue();
        ActiveCharacter.RefillActionPoints();
        yield return StartCoroutine(cameraController.SwitchTarget(ActiveCharacter.transform));
        gameUiManager.ActionWindow.ToggleWindow(true, true);
    }
}