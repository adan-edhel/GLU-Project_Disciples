using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class HUD : MonoBehaviourPunCallbacks, ITogglePause
{
    [SerializeField] TextMeshProUGUI scoreList;

    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        UpdateScoreList();
    }

    private void UpdateScoreList()
    {
        if (!SceneController.Instance.inMenu && scoreList != null)
        {
            scoreList.text = MatchManager.Instance.ScoreList;
        }
    }

    public void TogglePause(bool toggle)
    {
        pauseMenu.SetActive(toggle);
    }

    public void ChangeScene(int index)
    {
        TogglePause(false);
        SceneController.Instance.TransitionScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
