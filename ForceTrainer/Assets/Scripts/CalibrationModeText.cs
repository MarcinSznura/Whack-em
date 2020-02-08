using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CalibrationModeText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI label;
  
    public void OnPointerEnter(PointerEventData eventData)  
    {
        label.enabled = true;
        Debug.Log("Im over this");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        label.enabled = false;
    }
}
