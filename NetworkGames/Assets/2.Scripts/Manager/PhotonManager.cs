using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;
using PN = Photon.Pun.PhotonNetwork;

public class PhotonManager : PunManagerBase
{
    private RoomOptions _roomOption = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 2 };

    private int roomsCount = 0;
    private List<Define.RoomData> rooms = new List<Define.RoomData>();
    private Define.RoomData roomData = new Define.RoomData();

    private Define.RoomData _nowRoomData = new Define.RoomData();
    private bool _isLeft, _isMaster;
    private string _name, _roomName;

    private PhotonView _pv;

    public override void Init() { _pv = Util.GetOrAddComponent<PhotonView>(gameObject); }
    //_clientState = PN.NetworkClientState.ToString();

    public void Connect()
    {
        PN.ConnectUsingSettings();
        PN.AutomaticallySyncScene = true;
    }

    public void Disconnect()
    {
        PN.Disconnect();
    }

    public void JoinLobby()
    {
        PN.JoinLobby();
    }

    public void LoadScene(string name)
    {
        if(IsMaster())
        {
            PN.LoadLevel(name);
        }
        //_pv.RPC("LoadSceneRPC", RpcTarget.All, scene);
    }

    [PunRPC]
    public void LoadSceneRPC(Define.Scene scene)
    {
        SM.LoadScene(System.Enum.GetName(typeof(Define.Scene), scene));
    }

    public void CreateRoom(string roomName, string nickName)
    {
        if (PN.InRoom)
            return;

        _name = nickName;
        PN.NickName = _name;
        _roomName = roomName;
        PN.CreateRoom(_roomName, _roomOption);
    }

    public void JoinRoom(string name, string roomName)
    {
        _name = name;
        PN.NickName = _name;
        _roomName = roomName;
        PN.JoinRoom(_roomName);
    }

    public void JoinOrCreateRoom()
    {
        PN.JoinOrCreateRoom(roomData._name, _roomOption, null);
    }

    public void JoinRandomRoom()
    {
        PN.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PN.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        JoinLobby();
    }

    public override void OnConnectedToMaster()
    {
        //PN.LocalPlayer.NickName = _clientName;
        JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause) {}

    public override void OnJoinedLobby()
    {
        _lobby.OnUpdateRoomList();
    }

    public override void OnCreatedRoom()
    {
        _isMaster = PN.IsMasterClient;
        _isLeft = true;
        _pv.RPC("ShowInRoomPopup", RpcTarget.All, _isLeft, _name, IsMaster());
    }

    public override void OnJoinedRoom()
    {
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _pv.RPC("ShowInRoomPopup", RpcTarget.All, _isLeft, _name, IsMaster());
        _pv.RPC("SyncSide", RpcTarget.Others, _isLeft);
    }

    [PunRPC]
    private void ShowInRoomPopup(bool isLeft, string name, bool isMaster)
    {
        _lobby.ShowInRoomPopup(isLeft, name, isMaster);
    }

    [PunRPC]
    private void SyncSide(bool isLeft)
    {
        _isLeft = !isLeft;
        _pv.RPC("ShowInRoomPopup", RpcTarget.All, _isLeft, _name, IsMaster());
    }

    private string GetName()
    {
        return _name;
    }

    public bool IsMaster()
    {
        _isMaster = PN.IsMasterClient;
        return _isMaster;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _lobby.UnshowInRoomPopup();
    }

    public void RoomBoom()
    {
        foreach(Define.RoomData info in rooms)
        {
            if(info._name == _roomName)
            {
                rooms.Remove(info);
                break;
            }
        }
        _lobby.OnUpdateRoomList();
    }

    public Define.RoomData GetRoomInfo()
    {
        _nowRoomData._maxCount = PN.CurrentRoom.MaxPlayers;
        _nowRoomData._curCount = PN.CurrentRoom.PlayerCount;
        _nowRoomData._name = _roomName;
        return _nowRoomData;
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { }

    public override void OnJoinRoomFailed(short returnCode, string message) {  }

    public override void OnJoinRandomFailed(short returnCode, string message) {  }

    private LobbyScene _lobby;
    public void SetLobby(LobbyScene lobby)
    {
        _lobby = lobby;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        rooms.Clear();
        roomsCount = roomList.Count;
        for (int i = 0; i < roomsCount; i++)
        {
            if(roomList[i].PlayerCount == 0 || roomList[i].PlayerCount == roomList[i].MaxPlayers)
                continue;

            if (roomList[i].RemovedFromList)
                continue;

            if (i >= rooms.Count)
            {
                rooms.Add(new Define.RoomData());
            }
            roomData._name = roomList[i].Name;
            roomData._curCount = roomList[i].PlayerCount;
            roomData._maxCount = roomList[i].MaxPlayers;
            rooms[i] = roomData;
        }
        _lobby.OnUpdateRoomList();
    }

    public List<Define.RoomData> GetRoomList()
    {
        return rooms;
    }
}