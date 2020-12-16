using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void ChangeScene(int index)
    {
        SceneController.Instance.TransitionScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}