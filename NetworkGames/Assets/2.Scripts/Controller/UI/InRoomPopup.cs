using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InRoomPopup : MonoBehaviour
{
    private bool _isSideLeft;
    [SerializeField]
    private TMP_Text roomName, L_name, L_class;
    [SerializeField]
    private TMP_Text R_name, R_class;
    private Define.RoomData _nowRoom = new Define.RoomData();
    [SerializeField]
    private GameObject startButton;

    public void InitPopup(bool isLeft, string name, bool isMaster)
    {
        startButton.SetActive(Managers.Photon.IsMaster());

        _nowRoom = Managers.Photon.GetRoomInfo();

        roomName.text = _nowRoom._name;

        _isSideLeft = isLeft;
        if(_isSideLeft)
        {
            L_name.text = name;
            
            if(isMaster)
                L_class.text = "Host";
            else
                L_class.text = "Guest";
        }
        else
        {
            R_name.text = name;

            if (isMaster)
                R_class.text = "Host";
            else
                R_class.text = "Guest";
        }
    }

    public void Unshow()
    {
        L_class.text = "";
        R_class.text = "";
        L_name.text = "";
        R_name.text = "";
        gameObject.SetActive(false);
    }


    public void Exit()
    {
        Managers.Photon.LeaveRoom();
        gameObject.SetActive(false);
    }

    public void StartInGame()
    {
        Managers.Audio.StopBGM();
        Managers.Audio.PlaySFX(Define.SFX.Entry, transform);
        Managers.Scene.LoadScene(Define.Scene.InGame, false, true);
    }
}