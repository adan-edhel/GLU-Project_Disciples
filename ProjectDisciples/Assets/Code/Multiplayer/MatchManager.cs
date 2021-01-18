using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [SerializeField] private float _startHealth = 300f;
    [SerializeField] private List<IHealth> _aliveCharacters;
    [SerializeField] private List<GameObject> _characterObject;
    [SerializeField] private Dictionary<string, int> _score;
    [SerializeField] private string _coreList;
    [SerializeField] private List<PlayerHandler> _playerHandelers;
    bool _checkAllivePLayers;

    public string ScoreList
    {
        get { return _coreList; }
        set { _coreList = value; }
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
            _playerHandelers = new List<PlayerHandler>();
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(GetComponent<MatchManager>());
        }
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        _checkAllivePLayers = false;
    }


    private void FixedUpdate()
    {
        if (!SceneController.Instance.inMenu && PhotonNetwork.IsMasterClient && _checkAllivePLayers)
        {
            if (_aliveCharacters.Count == 1 && _characterObject.Count != (int)PhotonNetwork.CurrentRoom.PlayerCount)
            {
                _score[_aliveCharacters[0].GetPhotonView.Owner.NickName] += 1;
                _aliveCharacters[0].ResetHealth();
                ResetStage(); 
            }
        }
        
    }

    public void ResetStage()
    {
        _checkAllivePLayers = false;
        for (int i = 0; i < _playerHandelers.Count; i++)
        {
            _playerHandelers[i].RPCCreateCharacter();
        }
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
            if (_aliveCharacters.Count == _playerHandelers.Count)
            {
                _checkAllivePLayers = true;
            }
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

    public void RegisterPlayerHandler(PlayerHandler playerHandler)
    {
        if (!_playerHandelers.Contains(playerHandler))
        {
            _playerHandelers.Add(playerHandler);
            ReevaluateScoreBoard();
        }
    }

    public void RemovePlayerHandler(PlayerHandler playerHandler)
    {
        _playerHandelers.Remove(playerHandler);
        ReevaluateScoreBoard();
    }
}
