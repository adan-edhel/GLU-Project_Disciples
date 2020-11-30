using UnityEngine;

[CreateAssetMenu(fileName = "New Element Interaction", menuName = "ScriptableObjects/Element Interaction", order = 1)]
public class ElementInteraction : ScriptableObject
{
    [SerializeField] private EGameElement _FirstElement;
    [SerializeField] private EGameElement _SecondElement;
    [SerializeField] private float _multiplier = 1f;

    public EGameElement GetFirstElement
    { get { return _FirstElement; } }

    public EGameElement GetSecondElement
    { get { return _SecondElement; } }

    public float GetMultplier
    { get { return _multiplier; } }
}
