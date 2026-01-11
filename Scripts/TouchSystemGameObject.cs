using System;
using Unity.VisualScripting;
using UnityEngine;

public class TouchSystemGameObject : MonoBehaviour
{
    public static Action<GameObject> OnOstrichBeakTouch;
    private void OnTriggerEnter(Collider other)
    {
        OnOstrichBeakTouch.Invoke(other.GameObject());
    }
}
