using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
//using static UnityEngine.Rendering.DebugUI;

public class ObjectMovingBackwards : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnedObjects;
    // Храним созданные объекты
    private List<GameObject> movingObjects=new List<GameObject>();

    [Header("Скорость движения объектов")]
    public float moveSpeed = 2f;
    [SerializeField] private float angularSpeed = 0f;

    public static UnityEvent<GameObject> newObjCreated = new UnityEvent<GameObject>();
    public UnityEvent readyToCreateAnObject = new UnityEvent();
    
    private void Start()
    {
        ScoreCounter.OnTick += SetMoveSpeed;
        newObjCreated.AddListener(NewObjCreated);
        readyToCreateAnObject.AddListener(readyToCreateAnObjectVoid); 
        MotionControlManager.OnPlayerMovment += SetAngularSpeed;
    }
    private void OnDestroy()
    {
        ScoreCounter.OnTick-=SetMoveSpeed;
        newObjCreated.RemoveListener(NewObjCreated);
        readyToCreateAnObject.RemoveListener(readyToCreateAnObjectVoid); 
        MotionControlManager.OnPlayerMovment -= SetAngularSpeed;
    }

    private void Update()
    {
        if (spawnedObjects.Count > 0)
        {
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                ProcessingOfCreatedObjects(spawnedObjects[i]);
            }
        }
        // Движение всех объектов
        for (int i = movingObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = movingObjects[i];
            if (obj == null) continue;

            Vector3 moveDirection = Vector3.back * moveSpeed + Vector3.right * angularSpeed;

            //так это работало
            //obj.transform.position += moveDirection * Time.deltaTime;

            //так посоветовал сделать
            obj.GetComponent<Rigidbody>().MovePosition(obj.GetComponent<Rigidbody>().position += moveDirection * Time.deltaTime);

            if (obj.GetComponent<StoneSize>() && obj.transform.position.z >= 20f)
            {
                float t = Mathf.InverseLerp(45f, 20f, obj.transform.position.z); // t будет от 0 до 1, когда z от 45 до 20

                // Получаем размер от 0 до 0.5
                float scaleX = Mathf.Lerp(0f, obj.GetComponent<StoneSize>().sizeX, t);
                float scaleZ = Mathf.Lerp(0f, obj.GetComponent<StoneSize>().sizeZ, t);
                obj.transform.localScale=new Vector3(scaleX, 1, scaleZ);
            }


            // Удаление, если объект ушёл за границу
            if (obj.transform.position.z <= -20f)
            {
                //obj.transform.localScale=new Vector3(0, 0, 1);
                ObjectPool.GiveAwayTheObject(obj);
                movingObjects.RemoveAt(i);
            }
        }
    }
    void NewObjCreated( GameObject newObj)
    {
        spawnedObjects.Add(newObj);
    }
    private void ProcessingOfCreatedObjects (GameObject newObj)
    {
        newObj.transform.parent = transform;
        movingObjects.Add(newObj);
        spawnedObjects.Remove(newObj);
        if(newObj.GetComponent<StoneSize>())
            newObj.transform.rotation = transform.rotation;
    }
    void readyToCreateAnObjectVoid()
    {
        if(movingObjects.Count<Setings.maximumStonesOnTheMap)
            Spawner.newObjectMaximumNumberCheck?.Invoke((byte)movingObjects.Count);
    }

    void SetMoveSpeed(byte score)
    {
        moveSpeed = (byte)score;
    }
    void SetAngularSpeed(float score)
    {
        float scor = score * -1;
        float t = MathF.Abs(scor) * Mathf.InverseLerp(0f, 100f, moveSpeed); // t будет от 0 до 1, когда z от 0f до 100f
        float result = scor + MathF.Sign(scor) * t;
        angularSpeed =MathF.Round(result, 2);
    }
}
