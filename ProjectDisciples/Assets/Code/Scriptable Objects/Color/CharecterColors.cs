using UnityEngine;

[CreateAssetMenu(fileName = "New charecter Color", menuName = "ScriptableObjects/Character Color", order = 2)]
public class CharecterColors : ScriptableObject
{
    [SerializeField] private Color[] _cherecterColor;

    public Color[] getColors
    { get { return _cherecterColor; } }
}
