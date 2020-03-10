using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerPlacer : MonoBehaviour
{
    [SerializeField] Vector2 [] MarkersPositions;
    [SerializeField] Image Marker;

    [SerializeField] GameObject animated;
    [SerializeField] Canvas GUICanvas;
    [SerializeField] AudioClip sound;

    [SerializeField] Vector2 pos;
    private int newPlace, oldPlace;
    DetectColor colorDetector;
    GameMaster gameMaster;

    private void Awake()
    {
        colorDetector = FindObjectOfType<DetectColor>();
        gameMaster = FindObjectOfType<GameMaster>();
    }

    private void Update()
    {
        Vector2 m = new Vector2(Marker.rectTransform.localPosition.x, Marker.rectTransform.localPosition.y);
        if (colorDetector.TrackerOnMarker(m))
        {
            Whacked();
        }
    }

    public void Whacked()
    {
        oldPlace = newPlace;
        gameMaster.IncreaseScore(1);
        GetComponent<AudioSource>().PlayOneShot(sound);
        Instantiate(animated, new Vector3(Marker.transform.position.x, Marker.transform.position.y, Marker.transform.position.z), Quaternion.identity, GUICanvas.transform);
        PutMarkerInRandomTiles();
    }

    public  void PutMarkerInRandomTiles()
    {
        while(oldPlace == newPlace)
        {
            newPlace = Random.Range(0, 8);
        }
        
            pos = MarkersPositions[newPlace];
            Marker.rectTransform.localPosition = new Vector2(pos.x, pos.y);
    }

    public void HideMarker()
    {
        Marker.enabled = false;
    }

    public void ShowMarker()
    {
        Marker.enabled = true;
    }

    public void PutMarkerOutsideScreen()
    {
        Marker.rectTransform.localPosition = new Vector2(2000, 0);
    }

}
