using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SceneController : MonoBehaviourPunCallbacks
{
    public static SceneController Instance;

    [SerializeField] GameObject LoadingPanel;
    [SerializeField] TMP_Text loadAmountText;

    //private LoadingScreen loadingScreen;

    private int requestedIndex;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadingPanel.SetActive(false);

        //loadingScreen = GetComponent<LoadingScreen>();
    }

    /// <summary>
    /// Returns whether the game is in menu scene
    /// </summary>
    public bool inMenu => GetBuildIndex < 1;

    /// <summary>
    /// Returns the build index of the currently active scene
    /// </summary>
    /// <returns></returns>
    public int GetBuildIndex => SceneManager.GetActiveScene().buildIndex;

    /// <summary>
    /// Returns the total amount of scenes currently in Build Index
    /// </summary>
    /// <returns></returns>
    public int GetTotalBuildIndex => SceneManager.sceneCountInBuildSettings;

    /// <summary>
    /// Loads the scene specified with an int (Scene Index)
    /// </summary>
    /// <param name="sceneIndex"></param>
    //public void ChangeScene() => StartCoroutine(LoadSceneAsync(requestedIndex));

    /// <summary>
    /// Ends the application
    /// </summary>
    public void QuitGame() => Application.Quit();

    /// <summary>
    /// Transitions to a specified scene with a loading screen
    /// </summary>
    /// <param name="index"></param>
    public void TransitionScene(int index)
    {
        LoadingPanel.SetActive(true);

        requestedIndex = index;
        //loadingScreen.UpdateAnimatorValues(true, index);
        StartCoroutine(LoadSceneAsync(requestedIndex));
    }

    public void ResetScene()
    {
        requestedIndex = GetBuildIndex;
        //loadingScreen.UpdateAnimatorValues(true, requestedIndex);
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        PhotonNetwork.LoadLevel(index);

        //loadingScreen.ActivateElements();

        while (PhotonNetwork.LevelLoadingProgress < 1)
        {
            //loadingScreen.UpdateValues(Mathf.Clamp01(operation.progress / .9f));

            loadAmountText.text = "Loading: %" + (int)(PhotonNetwork.LevelLoadingProgress * 100);

            yield return new WaitForEndOfFrame();
        }

        LoadingPanel.SetActive(false);
        //loadingScreen.DisableElements();
        //loadingScreen.UpdateAnimatorValues(false, -1);
    }
}
