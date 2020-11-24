using UnityEngine;

[CreateAssetMenu(fileName = "New Element Interaction", menuName = "ScriptableObjects/Element Interaction", order = 1)]
public class ElementInteraction : ScriptableObject
{
    [SerializeField] private EPlayerElement _FirstElement;
    [SerializeField] private EPlayerElement _SecondElement;
    [SerializeField] private float _multiplier = 1f;

    public EPlayerElement GetFirstElement
    { get { return _FirstElement; } }

    public EPlayerElement GetSecondElement
    { get { return _SecondElement; } }

    public float GetMultplier
    { get { return _multiplier; } }
}
