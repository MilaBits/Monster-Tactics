using Dialog;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogEventListItem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text= default;

    public DialogEvent dialogEvent;

    public Button button;
    public Button upButton;
    public Button downButton;

    [SerializeField]
    private Sprite[] states = default;

    [SerializeField]
    private Image image= default;

    public void SetText(string text) => this.text.text = text;
    public void Select(bool value) => image.sprite = states[value ? 1 : 0];
}