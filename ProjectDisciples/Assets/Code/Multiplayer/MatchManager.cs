using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    [SerializeField] private float startHealth = 300f;
    [SerializeField] private List<IHealth> Characters;
    [SerializeField] private List<GameObject> characterObject;
    [SerializeField] private Dictionary<GameObject, int> Score;
    [SerializeField] private string _ScoreList;

    public string ScoreList
    {
        get { return _ScoreList; }
    }

    private void Awake()
    {
        Instance = this;
        Score = new Dictionary<GameObject, int>();
        Characters = new List<IHealth>();
        characterObject = new List<GameObject>();
    }

    private void Update()
    {
        if (SceneController.Instance.GetBuildIndex == 1)
        {
            List<GameObject> AlivePlayers = new List<GameObject>();
            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i]?.Health == 0)
                {
                    Characters[i].Health = -1f;
                    characterObject[i].transform.position = new Vector3(characterObject[i].transform.position.x, characterObject[i].transform.position.y, 0.2f);
                }
                else if (Characters[i]?.Health != -1f)
                {
                    AlivePlayers.Add(characterObject[i]);
                }
            }

            if (AlivePlayers.Count == 1)
            {
                Score[AlivePlayers[0]] += 1;
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
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].Health = startHealth;
            characterObject[i].transform.position = new Vector3(characterObject[i].transform.position.x, characterObject[i].transform.position.y, 0);
        }
        ReevaluateScoreBoard();
    }

    private void ReevaluateScoreBoard()
    {
        _ScoreList = "";
        foreach (var item in Score)
        {
            _ScoreList += ($"{item.Key.name} : {item.Value} points\n");
        }
    }

    public void RegisterCharacter(IHealth Health, GameObject Gameobject)
    {
        if (!Characters.Contains(Health))
        {
            Characters.Add(Health);
            characterObject.Add(Gameobject);
            Score.Add(Gameobject, 0);
            ReevaluateScoreBoard();
        }
    }

    public void RemoveCharacter(IHealth Health, GameObject Gameobject)
    {
        Characters.Remove(Health);
        characterObject.Remove(Gameobject);
        Score.Remove(Gameobject);
        ReevaluateScoreBoard();
    }
}
