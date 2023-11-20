using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoinButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _button, _setNickName;
    [SerializeField]
    private TMP_InputField _setNickName_Join;
    private string _name, _room;
    private RoomInfoBox _infoBox;

    public void OnClick()
    {
        _button.gameObject.SetActive(false);
        _setNickName.SetActive(true);
    }

    public void Show(Transform point, string room, RoomInfoBox infoBox)
    {
        _button.transform.position = point.position;
        _button.gameObject.SetActive(true);
        _infoBox = infoBox;
        _room = room;
    }

    public void UnShow()
    {
        _button.gameObject.SetActive(false);
    }

    public void Submit()
    {
        if (_setNickName_Join.text == "" || _setNickName_Join.text == "Enter Nick Name...")
        {
            Managers.UI.ShowNicknamePopup();
        }
        else
        {
            _name = _setNickName_Join.text;
            Managers.Photon.JoinRoom(_name, _room);
            _setNickName.SetActive(false);
        }
    }

    public void Exit()
    {
        _setNickName_Join.text = "";
        _infoBox.Exit();
        _setNickName.SetActive(false);
    }
}
