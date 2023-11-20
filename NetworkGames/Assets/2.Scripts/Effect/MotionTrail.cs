using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTrail : MonoBehaviour
{
    private float _maxDistance, _minDistance;
    private float _maxFade, _minFade;
    [SerializeField]
    private int _count;
    private Sprite _originSprite;
    private List<GameObject> _trails = new List<GameObject>();
    private bool _isOn;
    private Color _originColor, _fadeColor;

    private void Start()
    {
        OnTrail();

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        _originSprite = renderer.sprite;
        _originColor = renderer.color;
        _fadeColor = _originColor;
        _fadeColor.a = 0;

        for (int i = 0; i < _count; i++)
        {
            GameObject obj = new GameObject(name = $"trail{i + 1}");
            obj.transform.parent = transform;
            renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = _originSprite;
            renderer.color = _fadeColor;

            _trails.Add(obj);
        }
    }

    public void OffTrail()
    {
        _isOn = false;
    }

    public void OnTrail()
    {
        _isOn = true;
    }

    private void Update()
    {
        if (!_isOn)
            return;


    }
}
