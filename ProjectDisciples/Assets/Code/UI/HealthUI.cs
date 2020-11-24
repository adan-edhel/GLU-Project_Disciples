using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public float health;

    public float barDisplay;
    public Vector2 position = new Vector2(20, 40);
    public Vector2 size = new Vector2(60, 20);
    public Texture2D emptyText;
    public Texture2D fullText;

    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(position.x, position.y, size.x, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), emptyText);

        GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), fullText);
        GUI.EndGroup();
        GUI.EndGroup();
    }

    // Update is called once per frame
    private void Update()
    {
        barDisplay = health;
    }
}