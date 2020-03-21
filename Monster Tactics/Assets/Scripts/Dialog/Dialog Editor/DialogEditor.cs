using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Dialog;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace DefaultNamespace
{
    public class DialogEditor : MonoBehaviour
    {
        private DialogPlayer dialogPlayer;

        private DialogScript dialogScript = default;

        // Script UI

        [SerializeField]
        private TMP_Dropdown scriptDropdown = default;

        // Event List UI

        [SerializeField, FoldoutGroup("Event List UI")]
        private Transform eventList = default;

        [SerializeField, FoldoutGroup("Event List UI")]
        private DialogEventListItem listItem = default;

        [SerializeField, FoldoutGroup("Event List UI")]
        private Button addButton = default;

        [SerializeField, FoldoutGroup("Event List UI")]
        private Button removeButton = default;

        [SerializeField, FoldoutGroup("Event List UI")]
        private Button playButton = default;

        // event Editor UI

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_InputField durationInput = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_Dropdown characterDropdown = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private Toggle toggleL = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_Dropdown eventTypeDropdown = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_InputField targetXInput = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_InputField targetYInput = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private Button targetPickButton = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_InputField animTriggerInput = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_InputField spriteTriggerInput = default;

        [SerializeField, FoldoutGroup("Event UI")]
        private TMP_InputField textInput = default;

        private DialogEventListItem lastSelected;

        public void StartPlaying()
        {
            dialogPlayer.SetScript(dialogScript);
            dialogPlayer.Play();
        }

        private void NewSelection(DialogEventListItem selection)
        {
            if (lastSelected && selection.transform != lastSelected) lastSelected.Select(false);
            lastSelected = selection;

            selection.Select(true);

            durationInput.text = selection.dialogEvent.duration.ToString();

            toggleL.SetIsOnWithoutNotify(selection.dialogEvent.leftCharacter);
            // toggleR.isOn = !toggleL.isOn;

            eventTypeDropdown.SetValueWithoutNotify((int) selection.dialogEvent.type);
            eventTypeDropdown.RefreshShownValue();

            if (selection.dialogEvent.CharacterData)
            {
                int index = characterDropdown.options.FindIndex(
                    x => x.text.Equals(selection.dialogEvent.CharacterData.name));
                characterDropdown.SetValueWithoutNotify(index);
                characterDropdown.RefreshShownValue();
            }

            textInput.text = selection.dialogEvent.text;
            animTriggerInput.text = selection.dialogEvent.animationTrigger;
            spriteTriggerInput.text = selection.dialogEvent.spriteTrigger;

            targetXInput.text = selection.dialogEvent.target.x.ToString();
            targetYInput.text = selection.dialogEvent.target.y.ToString();

            ToggleInputs(selection.dialogEvent.type);
        }

        private void ToggleInputs(DialogEventType type)
        {
            toggleL.interactable = false;

            targetXInput.interactable = false;
            targetYInput.interactable = false;
            targetPickButton.interactable = false;

            animTriggerInput.interactable = false;
            spriteTriggerInput.interactable = false;

            textInput.interactable = false;
            characterDropdown.interactable = false;

            switch (type)
            {
                case DialogEventType.Wait:
                    break;
                case DialogEventType.Move:
                case DialogEventType.Attack:
                    characterDropdown.interactable = true;
                    toggleL.interactable = true;
                    targetXInput.interactable = true;
                    targetYInput.interactable = true;
                    targetPickButton.interactable = true;
                    break;
                case DialogEventType.Text:
                    characterDropdown.interactable = true;
                    toggleL.interactable = true;
                    textInput.interactable = true;
                    break;
                case DialogEventType.Anim:
                    characterDropdown.interactable = true;
                    toggleL.interactable = true;
                    animTriggerInput.interactable = true;
                    break;
                case DialogEventType.Sprite:
                    characterDropdown.interactable = true;
                    spriteTriggerInput.interactable = true;
                    toggleL.interactable = true;
                    break;
            }
        }

        private void Start()
        {
            dialogPlayer = FindObjectOfType<DialogPlayer>();
            if (!dialogPlayer) Debug.LogWarning("No dialog player found in scene!");


            UpdateScript(0);
            NewSelection(eventList.GetChild(0).GetComponent<DialogEventListItem>());
        }

        private void Awake()
        {
            addButton.onClick.AddListener(AddEvent);
            removeButton.onClick.AddListener(() => RemoveEvent(lastSelected.transform.GetSiblingIndex()));
            playButton.onClick.AddListener(StartPlaying);

            foreach (string asset in AssetDatabase.FindAssets("t:DialogScript"))
            {
                DialogScript data = AssetDatabase.LoadAssetAtPath<DialogScript>(AssetDatabase.GUIDToAssetPath(asset));
                scriptDropdown.options.Add(new TMP_Dropdown.OptionData(data.name));
            }

            durationInput.onValueChanged.AddListener(delegate(string arg0) { UpdateDuration(arg0); });

            scriptDropdown.onValueChanged.AddListener(UpdateScript);
            scriptDropdown.SetValueWithoutNotify(0);
            scriptDropdown.RefreshShownValue();

            foreach (object value in Enum.GetValues(typeof(DialogEventType)))
                eventTypeDropdown.options.Add(new TMP_Dropdown.OptionData(value.ToString()));
            eventTypeDropdown.onValueChanged.AddListener(delegate(int arg0) { UpdateEventType(arg0); });

            foreach (string asset in AssetDatabase.FindAssets("t:CharacterData"))
            {
                CharacterData data = AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(asset));
                characterDropdown.options.Add(new TMP_Dropdown.OptionData(data.name));
            }

            characterDropdown.onValueChanged.AddListener(delegate(int arg0)
            {
                UpdateCharacter(AssetDatabase.LoadAssetAtPath<CharacterData>(
                    $"Assets/Data/Characters/{characterDropdown.options[arg0].text}.asset"));
            });

            animTriggerInput.onEndEdit.AddListener(UpdateAnimTrigger);
            spriteTriggerInput.onEndEdit.AddListener(UpdateSpriteTrigger);

            targetXInput.onEndEdit.AddListener(delegate
            {
                UpdateTarget(new Vector2Int(int.Parse(targetXInput.text), int.Parse(targetYInput.text)));
            });
            targetYInput.onEndEdit.AddListener(delegate
            {
                UpdateTarget(new Vector2Int(int.Parse(targetXInput.text), int.Parse(targetYInput.text)));
            });

            textInput.onEndEdit.AddListener(UpdateText);

            toggleL.onValueChanged.AddListener(delegate(bool arg0)
            {
                lastSelected.dialogEvent.leftCharacter = arg0;
                UpdateScriptEvents();
                Debug.Log(lastSelected.dialogEvent.leftCharacter);
            });
        }

        private void UpdateDuration(string value)
        {
            if (!float.TryParse(value, out float result)) result = 0;
            lastSelected.dialogEvent.duration = result;
            UpdateScriptEvents();
        }

        private void UpdateEventType(int value)
        {
            lastSelected.dialogEvent.type = (DialogEventType) value;
            UpdateScriptEvents();
            DrawList();
            ToggleInputs(lastSelected.dialogEvent.type);
        }

        private void UpdateAnimTrigger(string value)
        {
            lastSelected.dialogEvent.animationTrigger = value;
            UpdateScriptEvents();
        }

        private void UpdateSpriteTrigger(string value)
        {
            lastSelected.dialogEvent.spriteTrigger = value;
            UpdateScriptEvents();
        }

        private void UpdateCharacter(CharacterData value)
        {
            lastSelected.dialogEvent.CharacterData = value;
            UpdateScriptEvents();
        }

        private void UpdateTarget(Vector2Int value)
        {
            lastSelected.dialogEvent.target = value;
            UpdateScriptEvents();
        }

        private void UpdateText(string value)
        {
            lastSelected.dialogEvent.text = value;
            UpdateScriptEvents();
        }

        private void UpdateScript(int i)
        {
            // dialogScript = AssetDatabase.LoadAssetAtPath<DialogScript>(
            //     AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(scriptDropdown.options[i].text)[0]));

            dialogScript = AssetDatabase.LoadAssetAtPath<DialogScript>($"Assets/Data/Dialog/{scriptDropdown.options[i].text}.asset");
            DrawList();
        }

        private void UpdateScriptEvents()
        {
            List<DialogEvent> events = GetEventsFromList();

            dialogScript.dialogEvents = new Queue<DialogEvent>(events);
        }

        private List<DialogEvent> GetEventsFromList()
        {
            List<DialogEvent> events = new List<DialogEvent>();
            for (int i = 0; i < eventList.childCount; i++)
            {
                events.Add(eventList.GetChild(i).GetComponent<DialogEventListItem>().dialogEvent);
            }

            return events;
        }

        private void DrawList()
        {
            for (int i = eventList.childCount; i-- > 0;) Destroy(eventList.GetChild(i).gameObject);
            foreach (DialogEvent dialogEvent in dialogScript.dialogEvents)
            {
                DialogEventListItem item = Instantiate(listItem, eventList);
                item.SetText(dialogEvent.type + " Event");
                item.dialogEvent = dialogEvent;

                item.button.onClick.AddListener(delegate { NewSelection(item); });
                item.upButton.onClick.AddListener(delegate { MoveItem(item, -1); });
                item.downButton.onClick.AddListener(delegate { MoveItem(item, 1); });
            }
        }

        private void MoveItem(DialogEventListItem item, int direction)
        {
            List<DialogEvent> events = dialogScript.dialogEvents.ToList();

            int index = events.FindIndex(x => x.Equals(item.dialogEvent));
            events.Swap(index, index + direction);
            dialogScript.dialogEvents = new Queue<DialogEvent>(events);
            DrawList();
        }

        private void AddEvent()
        {
            List<DialogEvent> events = dialogScript.dialogEvents.ToList();

            events.Add(new DialogEvent
            {
                text = string.Empty, animationTrigger = string.Empty,
                charInterval = .1f, duration = 0, target = Vector2Int.zero,
                type = DialogEventType.Wait, waitUntilDone = true, leftCharacter = true, spriteTrigger = "",
                CharacterData = lastSelected.dialogEvent.CharacterData
            });
            dialogScript.dialogEvents = new Queue<DialogEvent>(events);
            DrawList();
        }

        private void RemoveEvent(int index)
        {
            List<DialogEvent> events = dialogScript.dialogEvents.ToList();

            events.RemoveAt(index);
            dialogScript.dialogEvents = new Queue<DialogEvent>(events);
            DrawList();
        }
    }
}