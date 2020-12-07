﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void MoveToScene(int I) //TODO: Adjust to use scene controller in V2.0
    {
        SceneManager.LoadScene(I);
    }
}
