using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject stopGameButton;
    [SerializeField] GameObject calibrationModeButton;
    [SerializeField] GameObject switchWebcameraButton;
    [SerializeField] GameObject soundButton;
 
    [SerializeField] float captureRate = 1;
    [SerializeField] bool calibrationMode = false;
    [SerializeField] GameObject sliders;
    [SerializeField] Slider fpsValue;
    [SerializeField] TextMeshProUGUI fpsText;

    bool captureWebCamera = true;
    DetectColor colorReader;
    float roundTime = 0;
    bool soundOn = true;

    [SerializeField] Sprite soundOnImage;
    [SerializeField] Sprite soundOffImage;

    enum State {idlle, calibration, game };

    void Start()
    {
        colorReader = FindObjectOfType<DetectColor>();
        scoreText.text = "Score: " + score.ToString();
        timeText.text = "Time: " + Math.Round(roundTime, 2).ToString();
        state = State.idlle;
        fpsText.text = "FPS: " + fpsValue.value.ToString();
    }

    void Update()
    {
       // if (state == State.game)
        if (captureWebCamera)
            StartCoroutine(ReadColors());

        switch(state)
        {
            case State.idlle:
            
                break;

            case State.calibration:
             
                break;

            case State.game:
                roundTime -= Time.deltaTime;
                timeText.text = "Time: " + Math.Round(roundTime,2).ToString();
                if (roundTime <= 0)
                {
                    StopRound();
                }
                break;


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

        colorReader.ReadColors();

        yield return new WaitForSeconds(captureRate);
        captureWebCamera = true;
    }
    
    public void StartRound30()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkersInRandomTiles();
        roundTime = 30;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void StartRound1()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkersInRandomTiles();
        roundTime = 60;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void StartRound2()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkersInRandomTiles();
        roundTime = 120;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void StartRound5()
    {
        FindObjectOfType<MarkerPlacer>().PutMarkersInRandomTiles();
        roundTime = 360;
        score = 0;
        calibrationModeButton.SetActive(false);
        state = State.game;
        scoreText.text = "Score: " + score.ToString();
        HideShowRightButtons();
    }

    public void CloseGame()
    {
        Application.Quit();
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
            FindObjectOfType<DetectColor>().IsCalibrationModeOn(false);
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
            FindObjectOfType<DetectColor>().IsCalibrationModeOn(true);
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
