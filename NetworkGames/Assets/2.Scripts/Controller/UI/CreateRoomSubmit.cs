using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateRoomSubmit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_InputField _roomName, _nickName;
    
    [SerializeField]
    private GameObject _popup;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_roomName.text == "" || _roomName.text == "Enter Room Name...")
        {
            Managers.UI.ShowRoomNamePopup();
        }
        else if (_nickName.text == "" || _nickName.text == "Enter Nick Name...")
        {
            Managers.UI.ShowNicknamePopup();
        }
        else
        {
            Managers.Photon.CreateRoom(_roomName.text, _nickName.text);
            _popup.SetActive(false);
        }
    }
}
