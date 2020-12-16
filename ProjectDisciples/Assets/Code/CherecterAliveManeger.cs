using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CherecterAliveManeger : MonoBehaviour
{
    public static CherecterAliveManeger Instance;
    [SerializeField] private int _GameScene = 2;
    [SerializeField] private float _startHealth = 300f;
    [SerializeField] private List<IHealth> _cherecters;
    [SerializeField] private List<GameObject> _CherecterGameObjects;
    [SerializeField] private Dictionary<GameObject, int> _score;
    [SerializeField] private TMP_Text _ScoreBoard;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
        _score = new Dictionary<GameObject, int>();
        _cherecters = new List<IHealth>();
        _CherecterGameObjects = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (SceneController.Instance.GetBuildIndex == _GameScene)
        {
            List<GameObject> AlivePLayers = new List<GameObject>();
            for (int i = 0; i < _cherecters.Count; i++)
            {
                if (_cherecters[i]?.Health == 0)
                {
                    _cherecters[i].Health = -1f;
                    _CherecterGameObjects[i].transform.position = new Vector3(_CherecterGameObjects[i].transform.position.x, _CherecterGameObjects[i].transform.position.y, 0.2f);
                }
                else if (_cherecters[i]?.Health != -1f)
                {
                    AlivePLayers.Add(_CherecterGameObjects[i]);
                }
            }

            if (AlivePLayers.Count == 1 && AlivePLayers.Count != _CherecterGameObjects.Count)
            {
                _score[AlivePLayers[0]] += 1;
                ResetStage();
            }
        }
        else
        {
            Scanning();
        }
    }

    private void Scanning()
    {
        CharacterHealth[] Found = FindObjectsOfType<CharacterHealth>();
        for (int i = 0; i < Found.Length; i++)
        {
            addMe(Found[i], Found[i].gameObject);
        }
        revaluateScoreBoard();
    }

    public void ResetStage()
    {
        for (int i = 0; i < _cherecters.Count; i++)
        {
            _cherecters[i].Health = _startHealth;
            _CherecterGameObjects[i].transform.position = new Vector3(_CherecterGameObjects[i].transform.position.x, _CherecterGameObjects[i].transform.position.y, 0);
        }
        revaluateScoreBoard();
    }

    private void revaluateScoreBoard()
    {
        _ScoreBoard.text = "";
        foreach (var item in _score)
        {
            _ScoreBoard.text += ($"{item.Key.name} : {item.Value} points\n");
        }
    }

    public void addMe(IHealth Health, GameObject Gameobject)
    {
        if (!_cherecters.Contains(Health))
        {
            _cherecters.Add(Health);
            _CherecterGameObjects.Add(Gameobject);
            _score.Add(Gameobject, 0);
            revaluateScoreBoard();
        }
    }

    public void RemoveMe(IHealth Health, GameObject Gameobject)
    {
        _cherecters.Remove(Health);
        _CherecterGameObjects.Remove(Gameobject);
        _score.Remove(Gameobject);
        revaluateScoreBoard();
    }
}
