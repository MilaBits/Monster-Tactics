using System.Collections.Generic;
using Dialog;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog Script", menuName = "Monster Tactics/Dialog Script")]
public class DialogScript : SerializedScriptableObject
{
    [InfoBox("A new message is triggered by input.")]
    [SerializeField, ListDrawerSettings(CustomAddFunction = "AddEvent")]
    public Queue<DialogEvent> dialogEvents = new Queue<DialogEvent>();
    
    private void AddEvent()
    {
        dialogEvents.Enqueue(new DialogEvent
        {
            text = string.Empty, animationTrigger = string.Empty,
            charInterval = .1f, duration = 0, target = Vector2Int.zero,
            type = DialogEventType.Wait, waitUntilDone = true
        });
    }
}