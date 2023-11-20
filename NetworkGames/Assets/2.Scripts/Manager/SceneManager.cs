using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;

//씬 전환 기능을 담당하고 씬 정보를 갖고 있음

public class SceneManager : ManagerBase
{
    private SceneBase _scene;
    private Define.Scene _nextScene;
    private float _loadingTime;
    private bool _onLoading;
    private bool _isNet;

    public override void Init()
    {
        _isNet = false;
        _onLoading = false;
        _loadingTime = 3;
    }


    public void LoadScene(Define.Scene scene = Define.Scene.Load, bool isSkip = false, bool isNet = false)
    {
        if(isNet)
        {
            _isNet = isNet;
        }

        if(isSkip)
        {
            _nextScene = scene;
            _onLoading = true;
        }

        if (_onLoading)
        {
            string name = _nextScene.ToString();

            _onLoading = false;
            if(_isNet)
            {
                _isNet = false;
                Managers.Photon.LoadScene(name);
            }
            else
            {
                SM.LoadScene(name);
            }
        }
        else
        {
            _nextScene = scene;
            if (_isNet)
            {
                Managers.Photon.LoadScene("Load");
            }
            else
            {
                SM.LoadScene("Load");
            }
        }
    }

    public Define.Scene GetNextScene()
    {
        return _nextScene;
    }

    public float GetLoadTime()
    {
        return _loadingTime;
    }

    public void OnLoad()
    {
        _onLoading = true;
    }

    public void OnLoad(SceneBase scene)
    {
        _scene = scene;
    }
}