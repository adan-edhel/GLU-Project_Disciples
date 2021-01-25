using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Objectile : MonoBehaviourPunCallbacks
{
    [SerializeField] private EGameElement _Element;
    [SerializeField] private float _damageAmmound;
    [SerializeField] private GameObject _sender;
    string _senderName;
    [SerializeField] private LayerMask _playerLayers;
    Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public GameObject SetSender
    {
        set { _sender = value; }
    }


    private void FixedUpdate()
    {
        Collider2D[] Colliders = Physics2D.OverlapBoxAll(transform.position, transform.lossyScale * 1.01f, transform.rotation.z, _playerLayers);
        if (_sender != null)
        {
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].gameObject != _sender && Colliders[i].gameObject != gameObject)
                {
                    CharacterBase health = Colliders[i].GetComponent<CharacterBase>();
                    if (health != null)
                    {
                        health.DealDamage(_damageAmmound, _Element, photonView.Owner.NickName);
                    }
                    if (PhotonNetwork.InRoom)
                    {
                        photonView.RPC("destroyRPC", RpcTarget.All);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        Colliders = Physics2D.OverlapBoxAll(transform.position, transform.lossyScale * 1.01f, transform.rotation.z, ~_playerLayers);
        if (Colliders.Length > 0)
        {
            if (PhotonNetwork.InRoom)
            {
                photonView.RPC("destroyRPC", RpcTarget.All);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (photonView.IsMine)
        {
            transform.up = _rigidbody.velocity;
        }
        
    }

    public void SetStartingData(int layer, GameObject Sender)
    {
        gameObject.layer = layer;
        _sender = Sender;
    }

    public void SendRPC()
    {
        if (_sender != null)
        {
            if (_rigidbody != null)
            {
                photonView.RPC("setData", RpcTarget.Others, _rigidbody.velocity.x, _rigidbody.velocity.y, gameObject.layer, _sender.name);
            }
            DestroyAfter(5f);
        }
    }

    [PunRPC]
    public void destroyRPC()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void setData(float velocityX, float velocityY, int Layer, string SenderName)
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.velocity = new Vector2(velocityX, velocityY);
        }
        gameObject.layer = Layer;
        _senderName = SenderName;
        if (_sender == null || _senderName != _sender.name)
        {
            _sender = GameObject.Find(_senderName);
        }
    }

    private IEnumerator DestroyAfter(float Time)
    {
        yield return new WaitForSeconds(Time);
        photonView.RPC("destroyRPC", RpcTarget.All);
    }
}
