using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewRoom : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_InputField _roomName, _ninkName;
    [SerializeField]
    private GameObject _popup;
    private bool _showPopup = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        _showPopup = !_showPopup;

        _roomName.text = "";
        _ninkName.text = "";
        _popup.SetActive(_showPopup);
    }
}
