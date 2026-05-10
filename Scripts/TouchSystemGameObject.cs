using System;
using Unity.VisualScripting;
using UnityEngine;

public class TouchSystemGameObject : MonoBehaviour
{
    [SerializeField] private bool isUp = false;
    private void OnEnable()
    {
        Jamp.HeIsFly += JampEvent;
    }
    private void OnDisable()
    {
        Jamp.HeIsFly -= JampEvent;
    }
    private void JampEvent( bool state)
    {
        isUp=state;
    }
    public static Action<GameObject> OnOstrichBeakTouch;
    private void OnTriggerEnter(Collider other)
    {
        if(isUp==false)
            OnOstrichBeakTouch.Invoke(other.GameObject());
    }
}
