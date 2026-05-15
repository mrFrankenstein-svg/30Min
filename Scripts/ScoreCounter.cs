using System;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private static ScoreCounter scoreCounter;
    [SerializeField] byte score=1;
    private float counter;
    public static Action<byte> OnTick;
    [SerializeField] private TMP_Text scoreText;
    private void Start()
    {
        TMProTextManager.ChangeText(score, scoreText);
        scoreCounter = this;
    }
    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 1)
        {
            score++;
            counter = 0;
            OnTick?.Invoke(score);
            TMProTextManager.ChangeText(score, scoreText);
        }
    }
    public static byte GetScore()
    {
        return scoreCounter.score;
    }
}
