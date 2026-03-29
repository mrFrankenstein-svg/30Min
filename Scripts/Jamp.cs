using System;
using System.Collections;
using UnityEngine;

public class Jamp: MonoBehaviour
{
    public static Action<bool> HeIsFly;
    private bool isUp = false;
    private void Update()
    {
        if (Input.GetAxisRaw("Jump") != 0)
        {
            JampUp();
        }
    }
    private void JampUp()
    {
        if (isUp == false)
        {
            isUp = true;
            HeIsFly?.Invoke(isUp);
            StartCoroutine(Fly());
        }
    }
    private IEnumerator Fly()
    {
        tra
        yield return new WaitForSeconds(11);

    }
}
