using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DetectColor : MonoBehaviour
{
    [Header("WebCameras Info")]
    public WebCamTexture webCamTexture;
    WebCamDevice[] devices;
    [SerializeField] int width, height;

    [Header("RGB Colors")]
    [SerializeField] [Range(0, 1)] float HvalueMin = 0.1f;
    [SerializeField] [Range(0, 1)] float HvalueMax = 0.9f;
    [SerializeField] [Range(0, 1)] float SvalueMin = 0.1f;
    [SerializeField] [Range(0, 1)] float SvalueMax = 0.9f;
    [SerializeField] [Range(0, 1)] float VvalueMin = 0.1f;
    [SerializeField] [Range(0, 1)] float VvalueMax = 0.9f;

    [Header("Picture Details")]
    [SerializeField] [Range(0, 100)] float ObjectsDetectionRange = 20;
    public List<Vector2> middlePoints;
    public AllGroups ListOfAllGroups = new AllGroups();
    [SerializeField] bool CalibrationMode = false;

    [Header("Mapa")]
    public int[,] mapa;
    int mapaX = 1380, mapaY = 820;
    int nextgroup = 2;

    [Header("Sliders Value")]
    [SerializeField] GameObject sliders;
    public Slider sliderHvalueMin;
    public Slider sliderHvalueMax;
    public Slider sliderSvalueMin;
    public Slider sliderSvalueMax;
    public Slider sliderVvalueMin;
    public Slider sliderVvalueMax;
    public Slider sliderRange;

    [Header("Trackers")]
    public Image Tracker1;
    public Image Tracker2;

    [System.Serializable]
    public class SingleGroup
    {
        public List<Vector2Int> list;
    }

    [System.Serializable]
    public class AllGroups
    {
        public List<SingleGroup> list;
    }

    void Start()
    {
        devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture();
        webCamTexture.deviceName = devices[0].name;
        webCamTexture.Play();

        width = webCamTexture.width;
        height = webCamTexture.height;

        GetComponent<RawImage>().texture = webCamTexture;

        mapa = new int[mapaX, mapaY];
    }

    public void SubmitSliderSetting()
    {
        HvalueMax = sliderHvalueMax.value;
        HvalueMin = sliderHvalueMin.value;
        SvalueMin = sliderSvalueMin.value;
        SvalueMax = sliderSvalueMax.value;
        VvalueMin = sliderVvalueMin.value;
        VvalueMax = sliderVvalueMax.value;
        //ObjectsDetectionRange = sliderRange.value;
    }

    


    public void ReadColors()
    {
        ListOfAllGroups.list.Clear();
        middlePoints.Clear();
        nextgroup = 2;

        Texture2D image = new Texture2D(webCamTexture.width, webCamTexture.height);
        image.SetPixels(webCamTexture.GetPixels());
        image.Apply();

            Texture2D newTex = new Texture2D(image.width, image.height);

            for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {

                    var rgb = image.GetPixel(x, y);
                    float H, S, V;
                    Color.RGBToHSV(rgb, out H, out S, out V);

                    if (H > HvalueMin && H < HvalueMax && S > SvalueMin && S < SvalueMax && V > VvalueMin && V < VvalueMax)
                    {
                        mapa[x, y] = 1;
                        //Color rgb = Color.black;
                        newTex.SetPixel(x, y, Color.green);
                    
                }
                    else
                    {
                        mapa[x, y] = 0;
                    newTex.SetPixel(x, y, Color.black);
                }

                }
            }

        if (CalibrationMode)
        {
            newTex.Apply();
            GetComponent<RawImage>().texture = newTex;
        }
        else
        {
            GetComponent<RawImage>().texture = webCamTexture;
        }


        #region(Grupowanie pixeli)
        for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {
                    if (mapa[x, y] == 1)
                    {

                        for (int z = 1; z < 10; z++)
                        {
                            try
                            {
                                if (mapa[x - z, y] > 1)
                                {
                                    mapa[x, y] = mapa[x - z, y];
                                    int index = mapa[x, y] - 2;
                                    ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                    break;
                                }

                                if (mapa[x + z, y] > 1)
                                {
                                    mapa[x, y] = mapa[x + z, y];
                                    int index = mapa[x, y] - 2;
                                    ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                    break;
                                }
                                if (mapa[x, y - z] > 1)
                                {
                                    mapa[x, y] = mapa[x, y - z];
                                    int index = mapa[x, y] - 2;
                                    ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                    break;
                                }
                                if (mapa[x, y + z] > 1)
                                {
                                    mapa[x, y] = mapa[x, y + z];
                                    int index = mapa[x, y] - 2;
                                    ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                // przy krawedziach catch bedzie dzialal
                            }
                        }

                        if (mapa[x, y] == 1)
                        {
                            ListOfAllGroups.list.Add(new SingleGroup());
                            int index = ListOfAllGroups.list.Count - 1;
                            ListOfAllGroups.list[index].list = new List<Vector2Int>();
                            ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                            mapa[x, y] = nextgroup;
                            nextgroup++;
                            // Debug.Log("nowa grupa");
                        }
                    }
                }
            } //END OF FOR
            #endregion


            #region(Łączenie grup pixeli)
            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                if (ListOfAllGroups.list[ll].list.Count > 0)
                {
                    for (int la = 0; la < ListOfAllGroups.list.Count; la++)
                    {
                        if (ll != la && ListOfAllGroups.list[la].list.Count > 0)
                        {
                            Vector2Int v1 = new Vector2Int(ListOfAllGroups.list[ll].list[0].x, ListOfAllGroups.list[ll].list[0].y);
                            foreach (Vector2Int point in ListOfAllGroups.list[la].list)
                            {
                                Vector2Int v2 = point;
                                if (Vector2Int.Distance(v1, v2) < ObjectsDetectionRange)
                                {
                                    ListOfAllGroups.list[ll].list.AddRange(ListOfAllGroups.list[la].list);
                                    ListOfAllGroups.list[la].list.Clear();
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            #endregion


            #region(Wyznaczenie środków grupy)
            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                int minX = 1300, minY = 880, maxX = 0, maxY = 0;
                if (ListOfAllGroups.list[ll].list.Count > 10)
                {
                    foreach (Vector2Int v in ListOfAllGroups.list[ll].list)
                    {
                        if (v.x < minX) minX = v.x;
                        if (v.y < minY) minY = v.y;
                        if (v.x > maxX) maxX = v.x;
                        if (v.y > maxY) maxY = v.y;
                    }
                    middlePoints.Add(new Vector2((minX + maxX) / 2, (minY + maxY) / 2));
                }

            }
        #endregion

        if (middlePoints.Count != 1 && middlePoints.Count != 2)
        {
            Tracker1.enabled = false;
            Tracker2.enabled = false;
        }
        else
        {
            Tracker1.enabled = true;
            Tracker2.enabled = true;
        }

        if (middlePoints.Count == 2)
        {
            Tracker1.rectTransform.localPosition = new Vector2(middlePoints[0].x, middlePoints[0].y);
            Tracker2.rectTransform.localPosition = new Vector2(middlePoints[1].x, middlePoints[1].y);
        }

        if (middlePoints.Count == 1)
        {

            Tracker2.enabled = false;
            if (middlePoints[0].y < height/2)
            {
                Tracker1.rectTransform.localPosition = new Vector2(middlePoints[0].x, ((1080f / -2) + middlePoints[0].y * (1080f / height)) );
            }
            else
            {
                //Debug.Log(middlePoints[0].y +" /2  * "+1080f/ height);
                Tracker1.rectTransform.localPosition = new Vector2(middlePoints[0].x, ((middlePoints[0].y - height/2) * (1080f / height) ) );
            }
            Tracker1.rectTransform.localPosition = new Vector2((1920f / -2) + (middlePoints[0].x * (1920f / width)), Tracker1.rectTransform.localPosition.y);
            


        }
       
        // GetComponent<RawImage>().raycastTarget = false;
        //GetComponent<RawImage>().raycastTarget = true;
    }

 
    public void SwitchMode()
    {
        if (CalibrationMode)
        {
            CalibrationMode = false;
            sliders.SetActive(false);
            FindObjectOfType<MarkerPlacer>().ShowMarkes();
        }
        else
        {
            CalibrationMode = true;
            sliders.SetActive(true);
            FindObjectOfType<MarkerPlacer>().HideMarkes();
        }
    }

    public bool TrackerOnMarker(Vector2 vec)
    {
        if (middlePoints.Count > 0)
        {
            Vector2 pom = new Vector2(Tracker1.rectTransform.localPosition.x, Tracker1.rectTransform.localPosition.y);
            if (Vector2.Distance(pom, vec) < 200)
            {
                return true;
            }
        }
        
        return false;
    }

    public void SwitchWebcamera()
    {
        webCamTexture.Stop();
        webCamTexture.deviceName = (webCamTexture.deviceName == devices[0].name) ? devices[1].name : devices[0].name;
        webCamTexture.Play();
    }



}
