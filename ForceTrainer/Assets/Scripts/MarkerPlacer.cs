using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerPlacer : MonoBehaviour
{
    [SerializeField] Vector2 [] MarkersPositions;
    [SerializeField] Image Marker1;
    [SerializeField] Image Marker2;

    [SerializeField] Vector2 pos1;
    [SerializeField] Vector2 pos2;
    
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            PutMarkersInRandomTiles();
        }
    }

    private void PutMarkersInRandomTiles()
    {
        //Vector2 pos1, pos2;
        pos1 = MarkersPositions[Random.Range(0,9)];
        pos2 = MarkersPositions[Random.Range(0, 9)];
        while (pos1 == pos2)
        {
            pos2 = MarkersPositions[Random.Range(0, 9)];
        }

        Marker1.rectTransform.localPosition =  new Vector2 (pos1.x, pos1.y);
        Marker2.transform.localPosition = new Vector2(pos2.x, pos2.y);

    }



}
