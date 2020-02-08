using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] float captureRate = 1;
    bool captureWebCamera = true;
    DetectColor colorReader;

    void Start()
    {
        colorReader = FindObjectOfType<DetectColor>();
    }

    void Update()
    {
        if (captureWebCamera)
            StartCoroutine(ReadColors());
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


}
