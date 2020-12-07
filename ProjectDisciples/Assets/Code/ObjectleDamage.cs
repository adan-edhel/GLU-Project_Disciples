using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectleDamage : MonoBehaviourPunCallbacks
{
    [SerializeField] private EGameElement _Element;
    [SerializeField] private float _damageAmmound;
    [SerializeField] private GameObject _sender;
    string _senderName;
    [SerializeField] private LayerMask _playerLayers;

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
                    CharacterHealth PH = Colliders[i].GetComponent<CharacterHealth>();
                    if (PH != null)
                    {
                        PH.DealDamage(_damageAmmound, _Element);
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
            Rigidbody2D RB2D = GetComponent<Rigidbody2D>();
            if (RB2D != null)
            {
                photonView.RPC("setData", RpcTarget.Others, RB2D.velocity.x, RB2D.velocity.y, gameObject.layer, _sender.name);
            }
            DestroyAfter(5f);
        }
    }

    [PunRPC]
    public void destroyRPC()
    {
        Destroy(gameObject);
        //PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void setData(float velocityX, float velocityY, int Layer, string SenderName)
    {
        Rigidbody2D RB2D = GetComponent<Rigidbody2D>();
        if (RB2D != null)
        {
            RB2D.velocity = new Vector2(velocityX, velocityY);
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
