using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMover : MonoBehaviour
{
    [SerializeField] float captureRate = 1;
    [SerializeField] float movingSpeed = 0.1f;
    bool isReady = true;
    CameraInput photoTaker;
    ReadColorMain hand1;

    void Start()
    {
        photoTaker = FindObjectOfType<CameraInput>();
        hand1 = FindObjectOfType<ReadColorMain>();
    }


    void Update()
    {
        if (isReady)
            StartCoroutine(ReadHand());

        if (hand1.middlePoints.Count > 0)
        MoveBall(hand1.middlePoints[0]);
    }

    private void MoveBall(Vector2 mid)
    {
        if (mid.x > 400) gameObject.transform.position = new Vector3 (gameObject.transform.position.x - (1 * movingSpeed * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z );
        if (mid.x < 230) gameObject.transform.position = new Vector3(gameObject.transform.position.x + (1 * movingSpeed * Time.deltaTime), gameObject.transform.position.y, gameObject.transform.position.z);

        if (mid.y >210) gameObject.transform.position = new Vector3(gameObject.transform.position.x , gameObject.transform.position.y + (1 * movingSpeed * Time.deltaTime), gameObject.transform.position.z);
        if (mid.y < 150) gameObject.transform.position = new Vector3(gameObject.transform.position.x , gameObject.transform.position.y - (1 * movingSpeed * Time.deltaTime), gameObject.transform.position.z);
    }


    IEnumerator ReadHand()
    {

        isReady = false;

        photoTaker.TakePhoto();
        hand1.ReadColors();

        yield return new WaitForSeconds(captureRate);
        isReady = true;

    }
}
