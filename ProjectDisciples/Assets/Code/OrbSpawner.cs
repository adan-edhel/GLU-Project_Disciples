using System.Collections;
using Photon.Pun;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private string[] _orbPrefabLocations;
    [SerializeField,Min(1f)] private float _downtime = 1f;
    [SerializeField] private GameObject _currenorb;

    private void Start()
    {
        StartCoroutine(OrbSpwaner());
    }

    private IEnumerator OrbSpwaner()
    {
        float Counter = 0;
        while (true)
        {
            if (!SceneController.Instance.inMenu && SpawnArea.Instance != null && PhotonNetwork.InRoom && _currenorb == null) Counter += Time.fixedDeltaTime;
            else if (Counter != 0f) Counter = 0f;

            if (Counter >= _downtime)
            {
                Counter = 0;
                _currenorb = PhotonNetwork.Instantiate(_orbPrefabLocations.ReturnRandom(), SpawnArea.Instance.RandomPosition, Quaternion.identity);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
