using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [SerializeField] private float _startHealth = 300f;
    [SerializeField] private List<IHealth> _characters;
    [SerializeField] private List<GameObject> _characterObject;
    [SerializeField] private Dictionary<GameObject, int> _score;
    [SerializeField] private string _coreList;

    public string ScoreList
    {
        get { return _coreList; }
    }

    private void Awake()
    {
        Instance = this;
        _score = new Dictionary<GameObject, int>();
        _characters = new List<IHealth>();
        _characterObject = new List<GameObject>();
    }

    private void Update()
    {
        if (SceneController.Instance.GetBuildIndex == 1)
        {
            List<GameObject> AlivePlayers = new List<GameObject>();
            for (int i = 0; i < _characters.Count; i++)
            {
                if (_characters[i]?.Health == 0)
                {
                    _characters[i].Health = -1f;
                    _characterObject[i].transform.position = new Vector3(_characterObject[i].transform.position.x, _characterObject[i].transform.position.y, 0.2f);
                }
                else if (_characters[i]?.Health != -1f)
                {
                    AlivePlayers.Add(_characterObject[i]);
                }
            }

            if (AlivePlayers.Count == 1 && _characterObject.Count != AlivePlayers.Count)
            {
                _score[AlivePlayers[0]] += 1;
                ResetStage();
            }
        }
        else
        {
            Scan();
        }
    }

    private void Scan()
    {
        CharacterBase[] Found = FindObjectsOfType<CharacterBase>();
        for (int i = 0; i < Found.Length; i++)
        {
            RegisterCharacter(Found[i], Found[i].gameObject);
        }
        ReevaluateScoreBoard();
    }

    public void ResetStage()
    {
        for (int i = 0; i < _characters.Count; i++)
        {
            _characters[i].Health = _startHealth;
            _characterObject[i].transform.position = new Vector3(_characterObject[i].transform.position.x, _characterObject[i].transform.position.y, 0);
        }
        ReevaluateScoreBoard();
    }

    private void ReevaluateScoreBoard()
    {
        _coreList = "";
        foreach (var item in _score)
        {
            _coreList += ($"{item.Key.name} : {item.Value} points\n");
        }
    }

    public void RegisterCharacter(IHealth Health, GameObject Gameobject)
    {
        if (!_characters.Contains(Health))
        {
            _characters.Add(Health);
            _characterObject.Add(Gameobject);
            _score.Add(Gameobject, 0);
            ReevaluateScoreBoard();
        }
    }

    public void RemoveCharacter(IHealth Health, GameObject Gameobject)
    {
        _characters.Remove(Health);
        _characterObject.Remove(Gameobject);
        _score.Remove(Gameobject);
        ReevaluateScoreBoard();
    }
}
