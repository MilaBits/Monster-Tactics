using TMPro;
using UnityEngine;

public class LabelValueField : MonoBehaviour
{
    private TextMeshProUGUI Label;
    private TextMeshProUGUI Value;

    public void Init(string label, string value)
    {
        Label.text = label;
        Value.text = value;
    }

    public void SetValue(string value)
    {
        Value.text = value;
    }
}