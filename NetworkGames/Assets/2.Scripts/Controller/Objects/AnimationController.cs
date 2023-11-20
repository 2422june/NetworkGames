using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _ani;

    public void Init()
    {
        _ani = Util.GetOrAddComponent<Animator>(gameObject);
    }

    public void PlayTrue(string name)
    {
        _ani.SetBool(name, true);
    }

    public void PlayFalse(string name)
    {
        _ani.SetBool(name, false);
    }

    public void PlayFloat(string name, float value)
    {
        _ani.SetFloat(name, value);
    }

    public void Play(string name)
    {
        _ani.SetTrigger(name);
    }
}
