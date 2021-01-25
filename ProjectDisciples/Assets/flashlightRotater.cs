using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
 
public class flashlightRotater : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _flashlicht;
    [SerializeField] private CharacterAim _characterAimGameobject;
    void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(_flashlicht);
            Destroy(GetComponent<flashlightRotater>());
        }
    }

    private void FixedUpdate()
    {
        _flashlicht.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(gameObject.transform.up, _characterAimGameobject.aimDirection));
    }
}
