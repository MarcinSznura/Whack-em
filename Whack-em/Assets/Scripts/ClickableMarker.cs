using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableMarker : MonoBehaviour
{

    [SerializeField] MarkerPlacer MP;

    public void Click()
    {
        MP.Whacked();
        Debug.Log("clik");
    }

}
