using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatterPanel : MonoBehaviour
{
    public static ChatterPanel Instance;
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private ScrollRect _scrollRec;

    private void Start()
    {
        _textField.text = string.Empty;
    }

    public void SetAsStatic() => Instance = this;

    public void SetMessage(string Message)
    {
        _textField.text += Message;
        _scrollRec.verticalNormalizedPosition = 0f;
    }
}
