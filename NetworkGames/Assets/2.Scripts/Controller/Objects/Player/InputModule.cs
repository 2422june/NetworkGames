using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModule : MonoBehaviour
{
    private bool _up, _down, _right, _left;
    private Vector3 _dir;

    public void Init()
    {
        _up = false;
        _down = false;
        _right = false;
        _left = false;
    }

    public void Cycle()
    {
        InputKey();
        SetDir();
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _up = !_up;

        if (Input.GetKeyDown(KeyCode.A))
            _down = !_down;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _right = !_right;
            _left = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _left = !_left;
            _right = false;
        }
    }

    private void SetDir()
    {
        _dir = Vector3.zero;
        if (_up)
            _dir.y = 1;
        
        if (_right)
            _dir.x = 1;

        if (_left)
            _dir.x = -1;
    }

    public Vector3 GetDir()
    {
        return _dir;
    }

    public bool[] GetKey()
    {
        bool[] keys = { _up, _down, _right, _left };
        return keys;
    }

    public bool IsAttack()
    {
        return _down;
    }
}
