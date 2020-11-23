using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectleDamage : MonoBehaviour
{
    [SerializeField] private EPlayerElement _Element;
    [SerializeField] private float _damageAmmound;
    [SerializeField] private GameObject _sender;
    [SerializeField] private LayerMask _playerLayers;

    public GameObject SetSender
    {
        set { _sender = value; }
    }


    private void FixedUpdate()
    {
        Collider2D[] Colliders = Physics2D.OverlapBoxAll(transform.position, transform.lossyScale * 1.01f, transform.rotation.z, _playerLayers);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject != _sender && Colliders[i].gameObject != gameObject)
            {
                Debug.Log("Hit", this);
            }
        }
        Colliders = Physics2D.OverlapBoxAll(transform.position, transform.lossyScale * 1.01f, transform.rotation.z, ~_playerLayers);
        if (Colliders.Length > 0)
        {
            Destroy(gameObject);
        }
    }
}
