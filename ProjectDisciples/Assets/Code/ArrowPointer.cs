using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://answers.unity.com/questions/799656/how-to-keep-an-object-within-the-camera-view.html

public class ArrowPointer : MonoBehaviour
{
    [SerializeField] private Vector3 _startlocation;
    [SerializeField] private Vector2 _minMax;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    Vector3 _offset = new Vector3(0, 0, 90);

    private void Start()
    {
        _startlocation = transform.localPosition;
    }

    void FixedUpdate()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.parent.position + _startlocation);
        if ((pos.x >= 0 && pos.x <= 1) && (pos.y >= 0 && pos.y <= 1))
        {
            if (_spriteRenderer.enabled) _spriteRenderer.enabled = false;        
        }
        else
        {
            if (!_spriteRenderer.enabled) _spriteRenderer.enabled = true;
            pos.x = Mathf.Clamp(pos.x, _minMax.x, _minMax.y);
            pos.y = Mathf.Clamp(pos.y, _minMax.x, _minMax.y);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
            transform.right = (transform.parent.position - transform.position);
        }
    }

}
