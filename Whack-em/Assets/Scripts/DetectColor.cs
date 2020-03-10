using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectColor : MonoBehaviour
{
    [Header("WebCameras Info")]
    public WebCamTexture webCamTexture;
    WebCamDevice[] devices;
    [SerializeField] int width, height;

    [Header("Resolution Info")]
    [SerializeField] int screenWidth;
    [SerializeField] int screenHeight;
    

    [Header("RGB Colors")]
    [SerializeField] [Range(0, 1)] double HvalueMin = 0.1f;
    [SerializeField] [Range(0, 1)] double HvalueMax = 0.9f;
    [SerializeField] [Range(0, 1)] double SvalueMin = 0.1f;
    [SerializeField] [Range(0, 1)] double SvalueMax = 0.9f;
    [SerializeField] [Range(0, 1)] double VvalueMin = 0.1f;
    [SerializeField] [Range(0, 1)] double VvalueMax = 0.9f;
    [SerializeField] TextMeshProUGUI redRange;
    [SerializeField] TextMeshProUGUI blueRange;
    [SerializeField] TextMeshProUGUI greenRange;

    [Header("Picture Details")]
    [SerializeField] [Range(0, 100)] float ObjectsDetectionRange = 20;
    [SerializeField] TextMeshProUGUI rangeValue;
    public List<Vector2> middlePoints;
    public AllGroups ListOfAllGroups = new AllGroups();
    public Vector2 trackerMiddlePoint;

    [Header("Mapa")]
    public int[,] mapa;
    int mapaX = 1380, mapaY = 820;
    int nextgroup = 2;

    [Header("Sliders Value")]
    public Slider sliderHvalueMin;
    public Slider sliderHvalueMax;
    public Slider sliderSvalueMin;
    public Slider sliderSvalueMax;
    public Slider sliderVvalueMin;
    public Slider sliderVvalueMax;
    public Slider sliderRange;

    [Header("Tracker")]
    public Image Tracker;

    bool CalibrationMode = false;
    Texture2D image;
    Texture2D newTex;


    #region(Lists)
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
    #endregion

    void Start()
    {
        devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture();
        webCamTexture.deviceName = devices[0].name;
        webCamTexture.Play();

        width = webCamTexture.width;
        height = webCamTexture.height;

        screenWidth = Screen.currentResolution.width;
        screenHeight = Screen.currentResolution.height;

        GetComponent<RawImage>().texture = webCamTexture;

        mapa = new int[mapaX, mapaY];

        SubmitSliderSetting();
    }

    public void SubmitSliderSetting()
    {
        HvalueMax = Math.Round(sliderHvalueMax.value,2);
        HvalueMin = Math.Round(sliderHvalueMin.value, 2);
        SvalueMin = Math.Round(sliderSvalueMin.value, 2);
        SvalueMax = Math.Round(sliderSvalueMax.value, 2);
        VvalueMin = Math.Round(sliderVvalueMin.value, 2);
        VvalueMax = Math.Round(sliderVvalueMax.value, 2);
        ObjectsDetectionRange = sliderRange.value;
        redRange.text = (HvalueMin * 100).ToString() + "% - " + (HvalueMax*100).ToString() + "%";
        greenRange.text = (SvalueMin*100).ToString() + "% - " + (SvalueMax*100).ToString() + "%";
        blueRange.text = (VvalueMin*100).ToString() + "% - " + (VvalueMax*100).ToString()+"%";
        rangeValue.text = "Objects size: "+ObjectsDetectionRange.ToString();
    }

    public void ReadColors()
    {
        ListOfAllGroups.list.Clear();
        middlePoints.Clear();
        nextgroup = 2;

        image = new Texture2D(webCamTexture.width, webCamTexture.height);
        image.SetPixels(webCamTexture.GetPixels());
        image.Apply();

        newTex = new Texture2D(image.width, image.height);

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {

                var rgb = image.GetPixel(x, y);
                float R, G, B;
                R = rgb.r;
                G = rgb.g;
                B = rgb.b;

                if (R >= HvalueMin && R <= HvalueMax && G >= SvalueMin && G <= SvalueMax && B >= VvalueMin && B <= VvalueMax)
                {
                    mapa[x, y] = 1;
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


        #region(Pixels grouping)
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
                            Debug.Log("Exception caught: " + e);
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
                    }
                }
            }
        }
        #endregion

        #region(Pixels group merging)
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

        #region(Middle point finding)
        int maxGroup = 1;
        for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
        {
            int minX = 1300, minY = 880, maxX = 0, maxY = 0;
            if (ListOfAllGroups.list[ll].list.Count > maxGroup)
            {
                foreach (Vector2Int v in ListOfAllGroups.list[ll].list)
                {
                    if (v.x < minX) minX = v.x;
                    if (v.y < minY) minY = v.y;
                    if (v.x > maxX) maxX = v.x;
                    if (v.y > maxY) maxY = v.y;
                }
                trackerMiddlePoint = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
                middlePoints.Add(new Vector2((minX + maxX) / 2, (minY + maxY) / 2));
                maxGroup = ListOfAllGroups.list[ll].list.Count;
            }
        }
        #endregion

        #region(Camera and screen resolution scaling)
        if (middlePoints.Count > 0)
        {
            Tracker.enabled = true;
            float x, y;

            if (trackerMiddlePoint.y < height / 2)
            {
                y = (((height / -2) + trackerMiddlePoint.y) * (screenHeight / height));
            }
            else
            {
                y= (trackerMiddlePoint.y - height / 2) * (screenHeight / height);
            }

            x = -((width / -2) + trackerMiddlePoint.x) * (screenWidth / width);

            Tracker.rectTransform.localPosition = new Vector2(x,y);
        }
        else
        {
            Tracker.enabled = false;
        }
        #endregion


    }


    public void CleanTexture()
    {
        Destroy(newTex);
        Destroy(image);
    }
  

    public bool TrackerOnMarker(Vector2 vec)
    {
        if (middlePoints.Count > 0)
        {
            Vector2 pom = new Vector2(Tracker.rectTransform.localPosition.x, Tracker.rectTransform.localPosition.y);
            if (Vector2.Distance(pom, vec) < 200)
            {
                return true;
            }
        }
        
        return false;
    }

    public void SwitchWebcamera()
    {
        try
        {
            if (devices.Length > 1)
            {
                webCamTexture.Stop();
                webCamTexture.deviceName = (webCamTexture.deviceName == devices[0].name) ? devices[1].name : devices[0].name;
                webCamTexture.Play();
            }
        }
        catch 
        {
            Debug.Log("Nie ma kamery");
        }
    }

    public void IsCalibrationModeOn(bool modeOn)
    {
        CalibrationMode = modeOn;
    }

}
