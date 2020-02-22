using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerPlacer : MonoBehaviour
{
    [SerializeField] bool doubleMarker = false;
    [SerializeField] Vector2 [] MarkersPositions;
    [SerializeField] Image Marker1;

    [SerializeField] GameObject animated;
    [SerializeField] Canvas GUICanvas;
    [SerializeField] AudioSource soundPlayer;
    [SerializeField] AudioClip sound;

    [SerializeField] Vector2 pos1;

    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Vector2 m = new Vector2(Marker1.rectTransform.localPosition.x, Marker1.rectTransform.localPosition.y);
        if (FindObjectOfType<DetectColor>().TrackerOnMarker(m))
        {
            FindObjectOfType<GameMaster>().IncreaseScore(1);
            soundPlayer.PlayOneShot(sound);
            Instantiate(animated,new Vector3(Marker1.transform.position.x, Marker1.transform.position.y, Marker1.transform.position.z),Quaternion.identity, GUICanvas.transform);
            PutMarkersInRandomTiles();
        }

    }

    public  void PutMarkersInRandomTiles()
    {
       
            pos1 = MarkersPositions[Random.Range(0, 8)];
            Marker1.rectTransform.localPosition = new Vector2(pos1.x, pos1.y);
           

    }

    public void HideMarker()
    {
        Marker1.enabled = false;
    }

    public void ShowMarker()
    {
        Marker1.enabled = true;
    }

    public void PutMarkerOutsideScreen()
    {
        Marker1.rectTransform.localPosition = new Vector2(2000, 0);

    }

}
