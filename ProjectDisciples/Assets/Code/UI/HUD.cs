using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class HUD : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI scoreList;

    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        scoreList.text = MatchManager.Instance.scoreList;
    }

    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void ChangeScene(int index)
    {
        SceneController.Instance.TransitionScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
