using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomInfoBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text _name, _memberCount;

    [SerializeField]
    private Transform _point;

    private bool _isOnClick = false;
    private string _roomTxt;

    public void SetRoomInfo(int max, int current, string room)
    {
        _memberCount.text = $"{current}/{max}";
        _roomTxt = room;
        _name.text = room;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isOnClick = !_isOnClick;
        if(_isOnClick)
        {
            Managers.UI.ShowJoin(_point, _roomTxt, this);
        }
        else
        {
            Managers.UI.UnshowJoin();
        }
    }

    public void Exit()
    {
        _isOnClick = false;
    }
}
