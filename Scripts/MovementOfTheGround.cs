using System;
using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public class MovementOfTheGround : MonoBehaviour
{
    [Tooltip("Материал, у которого будет анимироваться Offset по Y")]
    public Material targetMaterial;

    [Tooltip("Скорость прокрутки по Y")]
    public float scrollSpeed = 2f; 
    
    [Tooltip("Скорость прокрутки по X")]
    public float angularSpeed = 2f;

    private float currentOffsetY = 0f;
    private float currentOffsetX = 0f;
    private void Start()
    {
        MotionControlManager.OnPlayerMovment += SetAngularScrollSpeed;
        ScoreCounter.OnTick+=SetScrollSpeed;
    }
    private void OnDestroy()
    {
        ScoreCounter.OnTick-=SetScrollSpeed;
        MotionControlManager.OnPlayerMovment -= SetAngularScrollSpeed;
    }

    void Update()
    {
        if (targetMaterial == null) return;

        // Увеличиваем Offset по Y
        currentOffsetY += (scrollSpeed * 0.1f) * Time.deltaTime;
        currentOffsetX += (angularSpeed * 0.1f) * Time.deltaTime;

        // Сброс до 0, если достигли 1
        if (currentOffsetY >= 1f)
            currentOffsetY -= 1f;

        //if (currentOffsetX >= 1f)
        //    currentOffsetX -= 1f;

        // Получаем текущий Offset X (чтобы сохранить его)
        Vector2 currentOffset = targetMaterial.GetTextureOffset("_BaseMap");

        // Устанавливаем новый Offset
        targetMaterial.SetTextureOffset("_BaseMap", new Vector2(currentOffsetX, currentOffsetY));
    }

    void SetScrollSpeed(byte score)
    {
        scrollSpeed = (float)score;
    }
    void SetAngularScrollSpeed(float angular)
    {
        float t = Mathf.Abs(angular) * Mathf.InverseLerp(0f, 100f, scrollSpeed); // t будет от 0 до 1, когда z от 0f до 100f
        float result = angular + Mathf.Sign(angular) * t;
        angularSpeed = MathF.Round(result,2);
    }
}
