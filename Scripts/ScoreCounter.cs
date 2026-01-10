using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] byte score=1;
    private float counter;
    public static Action<byte> OnTick;
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 1)
        {
            score++;
            counter = 0;
            OnTick?.Invoke(score);
        }
    }
}
