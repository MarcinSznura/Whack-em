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
    [SerializeField] GameObject calibrationModeButton;
    [SerializeField] GameObject switchWebcameraButton;
    [SerializeField] TextMeshProUGUI startStopButtonText;
 
    [SerializeField] float captureRate = 1;
    [SerializeField] bool calibrationMode = false;
    [SerializeField] GameObject sliders;

    bool captureWebCamera = true;
    DetectColor colorReader;
    float roundTime = 0;

    enum State {idlle, calibration, game };

    void Start()
    {
        colorReader = FindObjectOfType<DetectColor>();
        scoreText.text = "Score: " + score.ToString();
        timeText.text = "Time: " + Math.Round(roundTime, 2).ToString();
        state = State.idlle;
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

    IEnumerator ReadColors()
    {
        captureWebCamera = false;

        colorReader.ReadColors();

        yield return new WaitForSeconds(captureRate);
        captureWebCamera = true;
    }

    public void StartStopRoundButton()
    {
        if (state == State.game)
        {
            startStopButtonText.text = "Start";
            StopRound();
        }
        else
        {
            startStopButtonText.text = "Stop";
            FindObjectOfType<MarkerPlacer>().PutMarkersInRandomTiles();
            roundTime = 30;
            score = 0;
            calibrationModeButton.SetActive(false);
            state = State.game;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void StopRound()
    {
        state = State.idlle;
        FindObjectOfType<MarkerPlacer>().PutMarkerOutsideScreen();
        calibrationModeButton.SetActive(true);
        roundTime = 0;
        timeText.text = "Time: " + Math.Round(roundTime, 2).ToString();
        startStopButtonText.text = "Start";
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
            FindObjectOfType<MarkerPlacer>().HideMarker();
        }
    }


}
