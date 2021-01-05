using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ChatterPanel : MonoBehaviour
{
    public static ChatterPanel Instance;
    [SerializeField] private TMP_Text _textField;

    private void Start()
    {
        _textField.text = string.Empty;
    }

    public void SetAsStatic() => Instance = this;

    public void SetMessage(string Message)
    {
        _textField.text += Message;
    }
}
