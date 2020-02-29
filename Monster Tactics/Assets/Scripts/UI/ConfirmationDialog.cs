using UI;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour
{
    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button cancelButton;

    private DialogResult result = DialogResult.None;
    public DialogResult Result => result;

    private void Start()
    {
        yesButton.onClick.AddListener(delegate { result = DialogResult.Yes; });
        cancelButton.onClick.AddListener(delegate { result = DialogResult.Cancel; });
    }
}