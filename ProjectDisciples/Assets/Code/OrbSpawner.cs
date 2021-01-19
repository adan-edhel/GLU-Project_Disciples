using System.Collections;
using Photon.Pun;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private string[] _orbPrefabLocations;
    [SerializeField] private int _spawnPerRound;
    [SerializeField,Min(1f)] private float _downtime = 1f;

    private void Start()
    {
        StartCoroutine(OrbSpwaner());
    }

    private IEnumerator OrbSpwaner()
    {
        float Counter = 0;
        while (true)
        {
            if (!SceneController.Instance.inMenu && SpawnArea.Instance != null && PhotonNetwork.InRoom) Counter += Time.fixedDeltaTime;
            else if (Counter != 0f) Counter = 0f;

            if (Counter >= _downtime)
            {
                Counter = 0;
                for (int i = 0; i < _spawnPerRound; i++)
                {
                    PhotonNetwork.Instantiate(_orbPrefabLocations.ReturnRandom(), SpawnArea.Instance.RandomPosition, Quaternion.identity);
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
