using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Button multiButton;

    [SerializeField] private Button backFromCredits;
    [SerializeField] private Button goToCredits;
    [SerializeField] private GameObject creditCanvas;

    //[SerializeField] private Button backFromOptions;
    //[SerializeField] private Button goToOptions;
    //[SerializeField] private GameObject optionsCanvas;

    [SerializeField] private GameObject mainMenuCanvas;

    private bool _creditsCanvas = false;
    private bool _optionsVanvas = false;

    private void Start()
    {
        multiButton.onClick.AddListener(GoToMultiLobby);

        backFromCredits.onClick.AddListener(CreditsUI);
        goToCredits.onClick.AddListener(CreditsUI);
        //backFromOptions.onClick.AddListener(BackFromOptionsBut);
        //goToOptions.onClick.AddListener(OptionsUI);
    }

    private void GoToMultiLobby()
    {
        SceneController.Instance.TransitionScene(1);
    }

    private void CreditsUI()
    {
        _creditsCanvas = !_creditsCanvas;

        creditCanvas.SetActive(_creditsCanvas);
        mainMenuCanvas.SetActive(!_creditsCanvas);
    }

    //private void OptionsUI()
    //{
    //    optionsCanvas.SetActive(true);
    //    mainMenuCanvas.SetActive(false);
    //}
}