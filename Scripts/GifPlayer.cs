using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GifPlayer : MonoBehaviour
{
    [Header("No more than 126 images")]
    [SerializeField] private List<Sprite> sprits;
    [SerializeField] private float swapTime;

    private float lastTime;
    private sbyte currentSpriteIndex=0;
    private SpriteRenderer renderer;

    private void Start()
    {
        renderer= gameObject.GetComponent<SpriteRenderer>();
        //StartCoroutine(ReturnToPoolAfterPlay());
    }
    //IEnumerator ReturnToPoolAfterPlay()
    void Update()
    {
        lastTime += Time.deltaTime;
        if (lastTime >= swapTime)
        {
            lastTime = 0;
            currentSpriteIndex++;
            if (currentSpriteIndex > sprits.Count || currentSpriteIndex > 126)
            {
                currentSpriteIndex = 0;
            }
            renderer.sprite = sprits[currentSpriteIndex];
        }
        //yield return new WaitForFixedUpdate(); 
    }
}
