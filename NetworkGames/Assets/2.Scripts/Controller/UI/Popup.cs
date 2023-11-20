using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Popup : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject _popup;

    public void OnPointerClick(PointerEventData eventData)
    {
        _popup.SetActive(false);
    }
}
