using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PN = Photon.Pun.PhotonNetwork;

public class InGameScene : SceneBase
{
    [SerializeField]
    private Transform point, Left, Right;

    private void Start()
    {
        Init();
        InitUI();
        SpawnPlayer();
    }

    protected override void Init()
    {
        _type = Define.Scene.InGame;
        _name = "InGame";
    }

    protected override void InitUI() { }

    private void SpawnPlayer()
    {
        if(Managers.Photon.IsMaster())
        {
            point = Left;
        }
        else
        {
            point = Right;
        }

        GameObject result = PN.Instantiate("Object/Player", point.position, Quaternion.identity);
    }

    protected override void OnLoad()
    {
        Managers.Scene.OnLoad(this);
    }
}
