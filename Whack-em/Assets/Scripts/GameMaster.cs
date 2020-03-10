using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [Header("Game stats")]
    [SerializeField] State state;
    [SerializeField] int score = 0;
    [SerializeField] float captureRate = 1;
    [SerializeField] bool calibrationMode = false;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI fpsText;

    [Header("Buttons")]
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject stopGameButton;
    [SerializeField] GameObject calibrationModeButton;
    [SerializeField] GameObject switchWebcameraButton;
    [SerializeField] GameObject soundButton;

    [Header("Sliders")]
    [SerializeField] GameObject sliders;
    [SerializeField] Slider fpsValue;

    [Header("Images")]
    [SerializeField] Sprite soundOnImage;
    [SerializeField] Sprite soundOffImage;

    bool captureWebCamera = true;
    float roundTime = 0;
    bool soundOn = true;

    DetectColor colorReader;

    enum State {idlle, calibration, game };

    void Start()
    {
        colorReader = FindObjectOfType<DetectColor>();
        scoreText.text = "Score: " + score.ToString();
        timeText.text = "Time: " + Math.Round(roundTime, 2).ToString();
        fpsText.text = "FPS: " + fpsValue.value.ToString();
        state = State.idlle;
    }

    void Update()
    {
        try
        {
            if (captureWebCamera)
                StartCoroutine(ReadColors());

            switch (state)
            {
                case State.idlle:

                    break;

                case State.calibration:

                    break;

                case State.game:
                    roundTime -= Time.deltaTime;
                    timeText.text = "Time: " + Math.Round(roundTime, 2).ToString();
                    if (roundTime <= 0)
                    {
                        StopRound();
                    }
                    break;


            }
        }
        catch(Exception e)
        {
            Debug.Log("Exception caught: " + e);
        }
    }

    public void IncreaseScore(int inc)
    {
        score += inc;
        scoreText.text = "Score: " + score.ToString();
    }

    public void SetFPS()
    {
        captureRate = 1 / fpsValue.value;
        fpsText.text = "FPS: "+ fpsValue.value.ToString();
    }

    IEnumerator ReadColors()
    {
        captureWebCamera = false;

        colorReader.CleanTexture();
        colorReader.ReadColors();

        yield return new WaitForSeconds(captureRate);
        captureWebCamera = true;
    }

    #region (Buttons)
    public void StartRound30()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkerInRandomTiles();
        roundTime = 30;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void StartRound1()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkerInRandomTiles();
        roundTime = 60;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void StartRound2()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkerInRandomTiles();
        roundTime = 120;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void StartRound5()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkerInRandomTiles();
        roundTime = 360;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void TurnSoundOnOff()
    {
        if (soundOn)
        {
            soundOn = false;
            FindObjectOfType<MarkerPlacer>().GetComponent<AudioSource>().volume = 0;
            soundButton.GetComponent<Image>().sprite = soundOffImage;
        }
        else
        {
            soundOn = true;
            FindObjectOfType<MarkerPlacer>().GetComponent<AudioSource>().volume = 1;
            soundButton.GetComponent<Image>().sprite = soundOnImage;
        }
    }

    public void StopRound()
    {
        state = State.idlle;
        FindObjectOfType<MarkerPlacer>().PutMarkerOutsideScreen();
        calibrationModeButton.SetActive(true);
        roundTime = 0;
        timeText.text = "Time: " + Math.Round(roundTime, 2).ToString();
        HideShowRightButtons();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
    #endregion

    private void HideShowRightButtons()
    {
        if (state == State.game)
        {
            startGameButton.SetActive(false);
            stopGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(true);
            stopGameButton.SetActive(false);
        }
    }


    public void SwitchMode()
    {
        if (calibrationMode)
        {
            colorReader.IsCalibrationModeOn(false);
            scoreText.text = "Score: " + score.ToString();
            timeText.text = "Time: " + Math.Round(roundTime, 2).ToString();

            calibrationMode = false;
            sliders.SetActive(false);
            startGameButton.SetActive(true);
            switchWebcameraButton.SetActive(false);
            soundButton.SetActive(false);
            FindObjectOfType<MarkerPlacer>().ShowMarker();
        }
        else
        {
            colorReader.IsCalibrationModeOn(true);
            scoreText.text = "";
            timeText.text = "";
            calibrationMode = true;
            sliders.SetActive(true);
            startGameButton.SetActive(false);
            switchWebcameraButton.SetActive(true);
            soundButton.SetActive(true);
            FindObjectOfType<MarkerPlacer>().HideMarker();
        }
    }


}
