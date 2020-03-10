using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class MarkerAnimator : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().SetBool("Touched", true);
    }

    private void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DeleteShield"))
        {
            Destroy(gameObject);
        }
    }

}
