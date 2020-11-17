using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerElement
{
    Fire = 0,
    Water,
    wind,
    earth,
}

public class PlayerAttackMenager : MonoBehaviour, ICharacterAttack
{
    [SerializeField] private List<EPlayerElement> _KnownElements;
    [SerializeField] private EPlayerElement _CurrentElement = EPlayerElement.Fire;
    [SerializeField] private Animator _animetor;

    private void Start()
    {
        if (_KnownElements == null)
        {
            _KnownElements = new List<EPlayerElement>();
        }
    }


    /// <summary>
    /// Sets the player element to the given element.
    /// </summary>
    /// <param name="Element"></param>
    /// <param name="AddToKnownElements">adds it to the known elements if it isn't in it.</param>
    public void SetElement(EPlayerElement Element, bool AddToKnownElements = false)
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
        int MaxLength = System.Enum.GetValues(typeof(EPlayerElement)).Length;
        int I = Random.Range(0, MaxLength * System.DateTime.Now.Millisecond) % MaxLength;
        SetElement((EPlayerElement)I, AddToKnownElements);
    }

    /// <summary>
    /// adds a new element to the known element List.
    /// </summary>
    public EPlayerElement AddKnownElement
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
            case EPlayerElement.Fire:
                FireAttack1();
                break;
            case EPlayerElement.Water:
                WaterAttack1();
                break;
            case EPlayerElement.wind:
                WindAttack1();
                break;
            case EPlayerElement.earth:
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
            case EPlayerElement.Fire:
                FireAttack2();
                break;
            case EPlayerElement.Water:
                WaterAttack2();
                break;
            case EPlayerElement.wind:
                WindAttack2();
                break;
            case EPlayerElement.earth:
                EarthAttack2();
                break;
            default:
                Debug.LogError("this is not posseble", this);
                break;
        }
    }

    #region Fire

    private void FireAttack1()
    {

    }

    private void FireAttack2()
    {

    }

    #endregion Fire

    #region Water
    private void WaterAttack1()
    {

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


