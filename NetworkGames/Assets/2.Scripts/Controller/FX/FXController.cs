using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    private Animator _ani;
    private Transform _target;

    public void Show()
    {
        transform.position = _target.position;
        _ani.SetTrigger("Trigger");
    }

    public void Init(Transform target)
    {
        _target = target;
        _ani = GetComponent<Animator>();
    }
}
