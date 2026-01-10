using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MotionControlManager : MonoBehaviour
{
    [SerializeField] float input;
    public static Action<float> OnPlayerMovment;
    void Update()
    {
        //Debug.Log(Input.GetAxis("Horizontal"));
        //input = Input.GetAxis("Horizontal");

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (input + Input.GetAxisRaw("Horizontal") <= 10 && input + Input.GetAxisRaw("Horizontal") >= -10) 
            {
                if ((input > 0 && Input.GetAxisRaw("Horizontal") < 0) || (input < 0 && Input.GetAxisRaw("Horizontal") > 0))
                    input = 0;
                input += Input.GetAxisRaw("Horizontal");
            }
            OnPlayerMovment?.Invoke(input);
        }
        else if (input != 0)
        {
            input = 0;
            OnPlayerMovment?.Invoke(input);
        }

    }
}
