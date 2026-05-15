using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AudioManager;

public class Jamp: MonoBehaviour
{
    [SerializeField] private float jumpDuration = 2f; // Общая длительность движения (вверх и вниз)
    [SerializeField] private float jumpHeight = 0.3f; // Общая длительность движения (вверх и вниз)
    [SerializeField] private List<GameObject> jumpedObj;

    private List<Coroutine> listOfCoroutine= new List<Coroutine>();
    public static Action<bool> HeIsFly;
    private bool isUp = false;
    private void Update()
    {
        if (Input.GetAxisRaw("Jump") != 0 && isUp == false)
        {
            isUp = true;
            if (EnduranceSystem.JumpRequest() == true)
                JampUp();
            else
                isUp = false;
        }
    }
    private void JampUp()
    {
        HeIsFly?.Invoke(isUp);
        StartCoroutine(BouncingObjects());
        Instance.Play(SoundType.UI, this.transform.position);
    }
    private IEnumerator BouncingObjects()
    {
        for (int i = 0; i < jumpedObj.Count; i++)
        {
            listOfCoroutine.Add(null);
        }

        for (int i = 0; i < jumpedObj.Count; i++)
        {
            listOfCoroutine[i] = StartCoroutine(Fly(jumpedObj[i]));
        }
        while (listOfCoroutine.Count != 0)
        {
            yield return null;
        }
        isUp = false;
        HeIsFly?.Invoke(isUp);
    }
    private IEnumerator Fly(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * (startPosition.y * jumpHeight);

        float halfDuration = jumpDuration / 2f;

        // Движение вверх (с замедлением)
        float elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / halfDuration;

            // Параболическая кривая: сначала быстро, потом медленно
            float parabolicProgress = 1 - Mathf.Pow(1 - progress, 2);

            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, parabolicProgress);
            yield return new WaitForFixedUpdate();
        }

        // Устанавливаем точную позицию в верхней точке
        obj.transform.position = targetPosition;

        // Движение вниз (с ускорением)
        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / halfDuration;

            // Параболическая кривая: сначала медленно, потом быстро
            float parabolicProgress = Mathf.Pow(progress, 2);

            obj.transform.position = Vector3.Lerp(targetPosition, startPosition, parabolicProgress);
            yield return new WaitForFixedUpdate();
        }

        // Устанавливаем точную начальную позицию
        obj.transform.position = startPosition;
        listOfCoroutine.Remove(listOfCoroutine.Last());
    }
}

