using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using PN = Photon.Pun.PhotonNetwork;

public class PhotonManager : PunManagerBase
{
    private string _clientState;
    private string _clientName;

    private RoomOptions _roomOption = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 2 };

    public struct RoomData
    {
        public string _name;
        public int _maxCount;
        public int _curCount;
    }
    private int roomsCount = 0;
    private List<RoomData> rooms = new List<RoomData>();
    private RoomData roomData = new RoomData();

    private void Update()
    {
        _clientState = PN.NetworkClientState.ToString();
    }

    public void Connect()
    {
        PN.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PN.Disconnect();
    }

    public void JoinLobby()
    {
        PN.JoinLobby();
    }

    public void CreateRoom()
    {
        PN.CreateRoom(roomData._name, _roomOption);
    }

    public void JoinRoom()
    {
        PN.JoinRoom(roomData._name);
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

    public override void OnConnectedToMaster()
    {
        PN.LocalPlayer.NickName = _clientName;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("연결 끊김.");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료.");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 만들기 완료.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 완료.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 만들기 실패.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 참가 실패.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방 랜덤 참가 실패.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomsCount = roomList.Count;
        for (int i = 0; i < roomsCount; i++)
        {
            if (i >= rooms.Count)
            {
                rooms.Add(new RoomData());
            }
            roomData._name = roomList[i].Name;
            roomData._curCount = roomList[i].PlayerCount;
            roomData._maxCount = roomList[i].MaxPlayers;
            rooms[i] = roomData;
        }
    }

    private void Info()
    {
        if (PN.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PN.CurrentRoom.Name);
            Debug.Log("현재 방 인원수 : " + PN.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대인원수 : " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PN.PlayerList.Length; i++) playerStr += PN.PlayerList[i].NickName + ", ";
            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("접속한 인원 수 : " + PN.CountOfPlayers);
            Debug.Log("방 개수 : " + PN.CountOfRooms);
            Debug.Log("모든 방에 있는 인원 수 : " + PN.CountOfPlayersInRooms);
            Debug.Log("로비에 있는지? : " + PN.InLobby);
            Debug.Log("연결됐는지? : " + PN.IsConnected);
        }
    }
}