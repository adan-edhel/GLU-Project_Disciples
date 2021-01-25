﻿using UnityEngine;
using Photon.Pun;

public class ElementalOrb : MonoBehaviourPunCallbacks
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _radius;
    [SerializeField] private EGameElement _element;

    private void FixedUpdate()
    {
        Collider2D[] Coll = Physics2D.OverlapCircleAll(transform.position, _radius, _layer);
        if (Coll.Length != 0)
        {
            CharacterAttack TempAttack = Coll[0].GetComponent<CharacterAttack>();
            if (TempAttack != null)
            {
                TempAttack.SetElement(_element, true);
                photonView.RPC("NetworkDestoyObject", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void NetworkDestoyObject()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
