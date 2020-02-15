using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerPlacer : MonoBehaviour
{
    [SerializeField] bool doubleMarker = false;
    [SerializeField] Vector2 [] MarkersPositions;
    [SerializeField] Image Marker1;
    [SerializeField] Image Marker2;

    [SerializeField] Vector2 pos1;
    [SerializeField] Vector2 pos2;

    private void Start()
    {
        //PutMarkersInRandomTiles();
    }

    private void Update()
    {
        Vector2 m = new Vector2(Marker1.rectTransform.localPosition.x, Marker1.rectTransform.localPosition.y);
        if (FindObjectOfType<DetectColor>().TrackerOnMarker(m))
        {
            FindObjectOfType<GameMaster>().IncreaseScore(1);
            PutMarkersInRandomTiles();
        }

    }

    public  void PutMarkersInRandomTiles()
    {
        if (doubleMarker)
        {
            Marker2.enabled = true;
            pos1 = MarkersPositions[Random.Range(0, 9)];
            pos2 = MarkersPositions[Random.Range(0, 9)];
            while (pos1 == pos2)
            {
                pos2 = MarkersPositions[Random.Range(0, 9)];
            }

            Marker1.rectTransform.localPosition = new Vector2(pos1.x, pos1.y);
            Marker2.transform.localPosition = new Vector2(pos2.x, pos2.y);
        }
        else
        {
            Marker2.enabled = false;
             pos1 = MarkersPositions[Random.Range(0, 9)];
            Marker1.rectTransform.localPosition = new Vector2(pos1.x, pos1.y);
            
        }

    }

    public void HideMarkes()
    {
        Marker1.enabled = false;
        Marker2.enabled = false;
    }

    public void ShowMarkes()
    {
        Marker1.enabled = true;
        Marker2.enabled = true;
    }

    public void PutMarkersOutsideScreen()
    {
        Marker1.rectTransform.localPosition = new Vector2(2000, 0);
        Marker2.rectTransform.localPosition = new Vector2(2000, 0);

    }

}
