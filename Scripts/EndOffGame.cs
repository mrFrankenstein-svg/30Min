using System;
using UnityEngine;
using TMPro;

public class EndOffGame : MonoBehaviour
{
    [SerializeField] private GameObject lossWinWindow;
    [SerializeField] private TMP_Text scoreText;
    public static Action EndOfGame;

    private void OnEnable()
    {
        EndOfGame += GameEnd;
    }
    private void OnDisable()
    {
        EndOfGame -= GameEnd;
    }
    private void Start()
    {
        if (lossWinWindow.activeInHierarchy == true)
        {
            lossWinWindow.SetActive(false);
        }
    }
    private void GameEnd()
    {
        Time.timeScale = 0f; //выключить время на время паузы
        lossWinWindow.SetActive(true);
        TMProTextManager.ChangeText(ScoreCounter.GetScore(),scoreText);
    }
    public static void ResumeTime()
    {
        if(Time.timeScale !=1f)
            Time.timeScale = 1f;
    }

}
