using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [SerializeField] private float _startHealth = 300f;
    [SerializeField] private List<IHealth> _aliveCharacters;
    [SerializeField] private List<GameObject> _characterObject;
    [SerializeField] private Dictionary<string, int> _score;
    [SerializeField] private string _coreList;

    public string ScoreList
    {
        get { return _coreList; }
    }

    public List<GameObject> GetCharacterObjects
    {
        get { return _characterObject; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _score = new Dictionary<string, int>();
            _aliveCharacters = new List<IHealth>();
            _characterObject = new List<GameObject>();
        }
        else
        {
            Destroy(GetComponent<MatchManager>());

        }
    }

    private void Update()
    {
        if (!SceneController.Instance.inMenu)
        {
            if (_aliveCharacters.Count == 1 && _characterObject.Count != (int)PhotonNetwork.CurrentRoom.PlayerCount)
            {
                _score[_aliveCharacters[0].GetPhotonView.Owner.NickName] += 1;
                ResetStage();
            }
        }
    }

    public void ResetStage()
    {
        SceneController.Instance.ResetScene();
        ReevaluateScoreBoard();
    }

    private void ReevaluateScoreBoard()
    {
        _coreList = "";
        foreach (var item in _score)
        {
            _coreList += ($"{item.Key} : {item.Value} points\n");
        }
    }

    public void RegisterCharacter(IHealth Health, GameObject Gameobject)
    {
        if (!_aliveCharacters.Contains(Health))
        {
            _aliveCharacters.Add(Health);
            _characterObject.Add(Gameobject);
            ReevaluateScoreBoard();
        }
    }

    public void RemoveCharacter(IHealth Health, GameObject Gameobject)
    {
        _aliveCharacters.Remove(Health);
        _characterObject.Remove(Gameobject);
        ReevaluateScoreBoard();
    }

    public void RegisterNickname(string nickname)
    {
        if (!_score.ContainsKey(nickname))
        {
            _score.Add(nickname, 0);
            ReevaluateScoreBoard();
        }
    }

    public void RemoveNickname(string nickname)
    {
        _score.Remove(nickname);
        ReevaluateScoreBoard();
    }
}
