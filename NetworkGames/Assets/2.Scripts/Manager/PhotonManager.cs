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
        Debug.Log("���� ����.");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�.");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("�� ����� �Ϸ�.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� �Ϸ�.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ����� ����.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ���� ����.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("�� ���� ���� ����.");
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
            Debug.Log("���� �� �̸� : " + PN.CurrentRoom.Name);
            Debug.Log("���� �� �ο��� : " + PN.CurrentRoom.PlayerCount);
            Debug.Log("���� �� �ִ��ο��� : " + PN.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PN.PlayerList.Length; i++) playerStr += PN.PlayerList[i].NickName + ", ";
            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("������ �ο� �� : " + PN.CountOfPlayers);
            Debug.Log("�� ���� : " + PN.CountOfRooms);
            Debug.Log("��� �濡 �ִ� �ο� �� : " + PN.CountOfPlayersInRooms);
            Debug.Log("�κ� �ִ���? : " + PN.InLobby);
            Debug.Log("����ƴ���? : " + PN.IsConnected);
        }
    }
}