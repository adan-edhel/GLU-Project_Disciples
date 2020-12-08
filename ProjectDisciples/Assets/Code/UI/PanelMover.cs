using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMover : MonoBehaviour
{
    [SerializeField] private GameObject _MainPanel;
    [SerializeField] private GameObject _ChosenPanel;

    public void MoveToPanel(bool MainPanel)
    {
        if (MainPanel)
        {
            _MainPanel.SetActive(true);
            _ChosenPanel.SetActive(false);
        }
        else if (!MainPanel)
        {
            _MainPanel.SetActive(false);
            _ChosenPanel.SetActive(true);
        }
    }

    public void MoveToScene(int index)
    {
        SceneController.Instance.TransitionScene(index);
    }
}
