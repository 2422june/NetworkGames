using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowUIModule : MonoBehaviour
{
    private Image[] _images = new Image[4];
    private Sprite[] _blackArrows = new Sprite[4];
    private Sprite[] _whiteArrows = new Sprite[4];

    private Vector3 _dir;

    private InputModule _input;
    private string[] _keys = { "Up", "Down", "Right", "Left" };

    private RectTransform _canvas;
    private float _maxScale, _minScale, _nowScale;
    private float _scaleTime = 0.2f, _speed;

    private bool[] _keyTrigers = new bool[4];

    public void Init()
    {
        _input = GetComponent<InputModule>();

        GameObject resource = Resources.Load<GameObject>("UI/WorldCanvas");
        GameObject result = Instantiate(resource, transform);
        result.GetComponent<RectTransform>().localPosition = Vector3.zero;
        _canvas = result.GetComponent<RectTransform>();
        Transform root = _canvas.transform.Find("Button");

        for (int i = 0; i < _keys.Length; i++)
        {
            _images[i] = root.Find(_keys[i]).GetComponent<Image>();
            _whiteArrows[i] = Resources.Load<Sprite>("UI/Arrow/White/" + _keys[i] + "Arrow");
            _blackArrows[i] = Resources.Load<Sprite>("UI/Arrow/Black/" + _keys[i] + "Arrow");
        }

        _maxScale = 0.01f;
        _nowScale = _maxScale;
        _minScale = 0.001f;
    }

    public void OnCycle()
    {
        OnImage();
        StartCoroutine(UpScale());
    }

    public void OffCycle()
    {
        StartCoroutine(DownScale());
    }

    private IEnumerator DownScale()
    {
        _canvas.localScale = Vector3.one * _maxScale;
        _nowScale = _maxScale;

        _speed = (_nowScale - _minScale) / _scaleTime;
        float goal = _minScale;

        while (goal < _nowScale)
        {
            _nowScale -= _speed * Time.deltaTime;
            _canvas.localScale = Vector3.one * _nowScale;
            yield return null;
        }
        _canvas.localScale = Vector3.one * goal;
        _nowScale = goal;
        OffImage();
    }

    private IEnumerator UpScale()
    {
        _canvas.localScale = Vector3.one * _minScale;
        _nowScale = _minScale;

        _speed = (_maxScale - _nowScale) / _scaleTime;
        float goal = _maxScale;

        while (goal > _nowScale)
        {
            _nowScale += _speed * Time.deltaTime;
            _canvas.localScale = Vector3.one * _nowScale;
            yield return null;
        }
        _canvas.localScale = Vector3.one * goal;
        _nowScale = goal;
    }

    private void OffImage()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].enabled = false;
        }
    }

    private void OnImage()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].enabled = true;
        }
    }

    public void Cycle()
    {
        SetDir();
        SetImage();
    }

    private void SetDir()
    {
        _dir = _input.GetDir();
        _keyTrigers = _input.GetKey();
    }

    private void SetImage()
    {
        for(int i = 0; i < _keyTrigers.Length; i++)
        {
            if (_keyTrigers[i])
            {
                _images[i].sprite = _whiteArrows[i];
            }
            else
            {
                _images[i].sprite = _blackArrows[i];
            }
        }
    }
}
