using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;



    public void IncreaseScore(int inc)
    {
        score += inc;
        scoreText.text = "Score: " + score.ToString();
    }
}
