using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolBarController : MonoBehaviour
{
    private Slider _bar;
    private float _value;


    void Start()
    {
        _bar = GetComponent<Slider>();
    }

    void Update()
    {
        
    }
}
