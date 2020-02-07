using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPosition : MonoBehaviour
{
    [SerializeField] float captureRate = 1;
    [SerializeField] float movingSpeed = 0.1f;
    bool isReady = true;
    DetectColor hand1;

    void Start()
    {
        hand1 = FindObjectOfType<DetectColor>();
    }

    void Update()
    {
        if (isReady)
            StartCoroutine(ReadHand());

    }


    IEnumerator ReadHand()
    {

        isReady = false;

        hand1.ReadColors();

        yield return new WaitForSeconds(captureRate);
        isReady = true;

    }
}
