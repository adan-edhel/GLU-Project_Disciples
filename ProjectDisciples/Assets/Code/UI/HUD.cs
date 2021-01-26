using CryptUI.Scripts;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class HUD : MonoBehaviourPunCallbacks, ITogglePause
{
    [SerializeField] private PhotonView _photonView;

    [SerializeField] TextMeshProUGUI scoreList;

    [SerializeField] GameObject pauseMenu;

    [SerializeField] Image elementIcon;
    [SerializeField] ResourceBarController healthController;
    [SerializeField] ResourceBarController manaController;


    void Update()
    {
        UpdateScoreList();
    }

    public void UpdateHUDStats(float maxHP, float hp, float maxMana, float mana)
    {
        float healthSliderValue = hp / maxHP;
        float manaSliderValue = mana / maxMana;

        healthController.ApplyValue(healthSliderValue);
        manaController.ApplyValue(manaSliderValue);
    }

    private void UpdateScoreList()
    {
        if (!SceneController.Instance.inMenu && scoreList != null)
        {
            scoreList.text = MatchManager.Instance.ScoreList;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("SetScoreBoard", RpcTarget.Others, MatchManager.Instance.ScoreList);
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
        MultiplayerFunctions.Instance.QuitMultiplayerRoom();
    }

    public void LeaveGame()
    {
        MultiplayerFunctions.Instance.LeaveMultiplayerRoom();
    }
}
