using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : SceneBase
{
    [SerializeField]
    private GameObject _view;
    private List<Define.RoomData> _rooms = new List<Define.RoomData>();
    private List<RoomInfoBox> _roomBoxes = new List<RoomInfoBox>();

    [SerializeField]
    private JoinButton _joinButton;

    [SerializeField]
    private GameObject _roomNamePopup, _nickNamePopup, _roomBoom;
    [SerializeField]
    private InRoomPopup _inRoomPopup;

    private void Start()
    {
        Init();
        InitUI();
        OnLoad();
    }

    protected override void Init()
    {
        _type = Define.Scene.Lobby;
        _name = "Lobby";
        Managers.Photon.SetLobby(this);
        Managers.UI.SetLobby(this);

        _roomNamePopup.SetActive(false);
        _nickNamePopup.SetActive(false);
        _joinButton.UnShow();

        Managers.Photon.Connect();
    }

    protected override void InitUI() { }


    protected override void OnLoad()
    {
        Managers.Scene.OnLoad(this);
        UnshowJoin();
        RoomBoxUpdate();

        Managers.Audio.PlayBGM(Define.BGM.Lobby);
    }

    public void OnUpdateRoomList()
    {
        RoomBoxUpdate();
    }

    private void RoomBoxUpdate()
    {
        _rooms = Managers.Photon.GetRoomList();
        
        foreach(RoomInfoBox box in _roomBoxes)
        {
            Destroy(box.gameObject);
        }
        _roomBoxes.Clear();

        for (int i = 0; i < _rooms.Count; i++)
        {
            if (_roomBoxes.Count < _rooms.Count)
            {
                GameObject source = Resources.Load<GameObject>("UI/RoomInfoBox");
                GameObject obj = Instantiate(source, _view.transform);
                _roomBoxes.Add(obj.GetComponent<RoomInfoBox>());
            }
            _roomBoxes[i].SetRoomInfo(_rooms[i]._maxCount, _rooms[i]._curCount, _rooms[i]._name);
        }
    }

    public void ShowJoin(Transform point, string room, RoomInfoBox infoBox)
    {
        _joinButton.Show(point, room, infoBox);
    }

    public void UnshowJoin()
    {
        _joinButton.UnShow();
    }

    public void ShowRoomNamePopup()
    {
        _roomNamePopup.SetActive(true);
    }

    public void ShowNicknamePopup()
    {
        _nickNamePopup.SetActive(true);
    }

    public void ShowInRoomPopup(bool isLeft, string name, bool isMaster)
    {
        _inRoomPopup.gameObject.SetActive(true);
        _inRoomPopup.InitPopup(isLeft, name, isMaster);
    }

    public void UnshowInRoomPopup()
    {
        Managers.Photon.LeaveRoom();
        Managers.Photon.RoomBoom();
        _inRoomPopup.Unshow();
        ShowRoomBoom();
    }

    public void ShowRoomBoom()
    {
        _roomBoom.SetActive(true);
    }
}
