using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MarkerPlacer))]
public class ClickableMarker : MonoBehaviour
{
    public void Click()
    {
        GetComponent<MarkerPlacer>().Whacked();
    }

}
