using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum EGameElement
{
    Fire = 0,
    Magma,
    Water,
    Ice,
    wind,
    earth,
}

public class PlayerAttackMenager : MonoBehaviour, ICharacterElement
{
    [SerializeField] private List<EGameElement> _KnownElements;
    [SerializeField] private EGameElement _CurrentElement = EGameElement.Fire;

    [Header("renderer")]
    [SerializeField] private SpriteRenderer _SpriteRenderer;
 
    [Header("fire")]
    [SerializeField] private string _FireFirstAtackPath = "Elements/Fire/BaseAtack";
    [SerializeField] private Vector2 _firevelocity;
    [SerializeField] private string _FireSecondAttackPath;

    [Header("Water")]
    [SerializeField] private string _WaterFirstAtackPath = "Elements/Water/BaseAtack";
    [SerializeField] private Vector2 _Watervelocity;
    [SerializeField] private string _WaterSecondAttackPath;

    private void Start()
    {
        if (_KnownElements == null)
        {
            _KnownElements = new List<EGameElement>();
        }
    }

    /// <summary>
    /// Sets the player element to the given element.
    /// </summary>
    /// <param name="Element"></param>
    /// <param name="AddToKnownElements">adds it to the known elements if it isn't in it.</param>
    public void SetElement(EGameElement Element, bool AddToKnownElements = false)
    {
        if (!_KnownElements.Contains(Element) && AddToKnownElements)
        {
            AddKnownElement = Element;
        }

        if (_KnownElements.Contains(Element))
        {
            _CurrentElement = Element;
        }
        else
        {
            Debug.LogError("element not in List", this);
        }
    }

    /// <summary>
    /// sets the player element to a Random element.
    /// </summary>
    /// <param name="AddToKnownElements">adds it to the known elements if it isn't in it.</param>
    public void SetElement(bool AddToKnownElements = false)
    {
        int MaxLength = System.Enum.GetValues(typeof(EGameElement)).Length;
        int I = UnityEngine.Random.Range(0, MaxLength * System.DateTime.Now.Millisecond) % MaxLength;
        SetElement((EGameElement)I, AddToKnownElements);
    }

    /// <summary>
    /// adds a new element to the known element List.
    /// </summary>
    public EGameElement AddKnownElement
    {
        set
        {
            _KnownElements.Add(value);
        }
    }


    public void Attack1()
    {
        switch (_CurrentElement)
        {
            case EGameElement.Fire:
                FireAttack1();
                break;
            case EGameElement.Water:
                WaterAttack1();
                break;
            case EGameElement.wind:
                WindAttack1();
                break;
            case EGameElement.earth:
                EarthAttack1();
                break;
            default:
                Debug.LogError("this is not posseble", this);
                break;
        }
    }

    public void Attack2()
    {
        switch (_CurrentElement)
        {
            case EGameElement.Fire:
                FireAttack2();
                break;
            case EGameElement.Water:
                WaterAttack2();
                break;
            case EGameElement.wind:
                WindAttack2();
                break;
            case EGameElement.earth:
                EarthAttack2();
                break;
            default:
                Debug.LogError("this is not posseble", this);
                break;
        }
    }

    public void SwitchPreviousElement()
    {
        int Current = (int)_CurrentElement;
        int Enumength = System.Enum.GetNames(typeof(EGameElement)).Length;
        for (int i = 1; i < Enumength; i++)
        {
            Current -= i;
            if (Current < 0)
            {
                Current = Enumength + Current;
            }
            if (_KnownElements.Contains((EGameElement)Current))
            {
                SetElement((EGameElement)Current);
                break;
            }
        }
    }

    public void SwitchNextElement()
    {
        int Current = (int)_CurrentElement;
        int Enumength = System.Enum.GetNames(typeof(EGameElement)).Length;
        for (int i = 1; i < Enumength; i++)
        {
            Current = (Current + i) % Enumength;
            if (_KnownElements.Contains((EGameElement)Current))
            {
                SetElement((EGameElement)Current);
                break;
            }
        }
    }

    #region Fire

    private void FireAttack1()
    {
        GameObject G;
        if (PhotonNetwork.InRoom)
        {
            G = PhotonNetwork.Instantiate(_FireFirstAtackPath, transform.position, Quaternion.identity);
        }
        else
        {
            G = Instantiate(Resources.Load(_FireFirstAtackPath) as GameObject, transform.position, Quaternion.identity);
        }
        G.layer = gameObject.layer;

        Rigidbody2D RB = G.GetComponent<Rigidbody2D>();
        if (RB != null)
        {
            Vector2 NewVelocity = new Vector2(_firevelocity.x * (_SpriteRenderer.flipX ? -1 : 1), _firevelocity.y);
            RB.velocity = NewVelocity;
        }

        ObjectleDamage OD = G.GetComponent<ObjectleDamage>();
        if (OD != null)
        {
            OD.SetSender = this.gameObject;
        }

        Destroy(G, 5);
    }

    private void FireAttack2()
    {

    }

    #endregion Fire

    #region Water
    private void WaterAttack1()
    {
        GameObject G;
        if (PhotonNetwork.InRoom)
        {
            G = PhotonNetwork.Instantiate(_WaterFirstAtackPath, transform.position, Quaternion.identity);
        }
        else
        {
            G = Instantiate(Resources.Load(_WaterFirstAtackPath) as GameObject, transform.position, Quaternion.identity);
        }
        G.layer = gameObject.layer;

        Rigidbody2D RB = G.GetComponent<Rigidbody2D>();
        if (RB != null)
        {
            Vector2 NewVelocity = new Vector2(_Watervelocity.x * (_SpriteRenderer.flipX ? -1 : 1), _Watervelocity.y);
            RB.velocity = NewVelocity;
        }

        ObjectleDamage OD = G.GetComponent<ObjectleDamage>();
        if (OD != null)
        {
            OD.SetSender = this.gameObject;
        }
        Destroy(G,5);
    }

    private void WaterAttack2()
    {

    }

    #endregion Water

    #region Wind

    private void WindAttack1()
    {

    }

    private void WindAttack2()
    {

    }

    #endregion wind

    #region Earth

    private void EarthAttack1()
    {

    }

    private void EarthAttack2()
    {

    }

    #endregion Eurth
}


