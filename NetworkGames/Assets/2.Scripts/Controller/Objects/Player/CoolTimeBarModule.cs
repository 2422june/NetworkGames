using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeBarModule : MonoBehaviour
{
    private float _time, _speed;
    private Slider _bar;
    private PlayerController _controller;
    private float _max;

    public void Init()
    {
        _controller= GetComponent<PlayerController>();

        _bar = GameObject.Find("Canvas").transform.Find("Bar").GetComponent<Slider>();
    }

    public void OnCycle()
    {
        SetMax(100);
        _bar.value = 0;

        SetTime(3f);
    }

    public void SetMax(int max)
    {
        _max = max;
        _bar.maxValue = _max;
    }

    public void SetTime(float time)
    {
        _time = time;
        _speed = _max / _time;
    }

    public void Cycle()
    {
        _bar.value += _speed * Time.deltaTime;
        if(_bar.value >= _bar.maxValue)
        {
            _bar.value = _bar.maxValue;
            OnReady();
        }
    }

    private void OnReady()
    {
        _controller.OnReady();
    }
}
