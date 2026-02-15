using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

interface IStoneSpawnerSlave
{
    GameObject prefab { get; }
    int chanceOfPrefab { get; }
    bool needToSizeChange { get; }
}
// public interface IStoneSpawnerSlave
//{
//    GameObject prefab { get; }
//    int chanceOfPrefab { get; }
//}

public class Spawner : MonoBehaviour
{
    [Header("Префаб для спавна")]
    [SerializeField] private List<StoneSpawnerSlave> prefabs;


    [Header("Интервал между спавнами (в секундах)")]
    public float spawnInterval = 1.5f;
    [SerializeField] float exponent = 2;
    [SerializeField] float X = 0.15f;
    [SerializeField] float Y = 45f;


    private float spawnTimer = 0f;

    public static UnityEvent<byte> newObjectMaximumNumberCheck= new UnityEvent<byte>();

    private void OnEnable()
    {
        ScoreCounter.OnTick+=SetSpawnTime;
        newObjectMaximumNumberCheck.AddListener(NewObjectMaximumNumberCheckVoid);
    }
    private void OnDisable()
    {
        ScoreCounter.OnTick-=SetSpawnTime;
        newObjectMaximumNumberCheck.RemoveListener(NewObjectMaximumNumberCheckVoid);
    }
    void Update()
    {
        // Спавн по таймеру
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnObject();
            spawnTimer = 0f;
        }
    }
    IStoneSpawnerSlave PrefabRandomizer()
    {
        //переместить в старт когда закончу
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].prefab == null)
            {
                Debug.LogErrorFormat(" StoneSpawner: prefabs[{0}]: prefab == null.", i);
                break;
            }
            if (prefabs[i].chanceOfPrefab <= 0)
            {
                Debug.LogErrorFormat(" StoneSpawner: prefabs[{0}]: chanceOfPrefab <= 0.", i);
                break;
            }
        }

        int maxChance = 0;

        for (int i = 0; i < prefabs.Count; i++)
        {
            maxChance += prefabs[i].chanceOfPrefab;
        }
        int chance = Random.Range(0, maxChance);

        int j = prefabs.Count - 1;

        for (; j > 0; j--)
        {
            if (chance >= maxChance - prefabs[j].chanceOfPrefab)
                break;
        }
        return prefabs[j];
    }

    void SpawnObject()
    {
        float randomX = Random.Range(-25f, 25f);
        Vector3 spawnPosition = new Vector3(randomX, X, Y);
        IStoneSpawnerSlave spawnedObj = PrefabRandomizer();
        GameObject obj = null;
        obj = ObjectPool.GetAnObject(spawnedObj.prefab.name);
        obj.transform.position = spawnPosition;
        obj.transform.localScale = new Vector3(0, 0, 0);

        if (spawnedObj.needToSizeChange)
        {
            obj.GetComponent<StoneSize>().sizeX = Random.Range(Setings.minStoneSize, Setings.maxStoneSize);
            obj.GetComponent<StoneSize>().sizeZ = Random.Range(Setings.minStoneSize, Setings.maxStoneSize);
        }
        ObjectMovingBackwards.newObjCreated.Invoke(obj);
    }
    public void NewObjectMaximumNumberCheckVoid(byte checkNumber)
    {
        if (checkNumber > Setings.maximumStonesOnTheMap)
            SpawnObject();
    }
    void SetSpawnTime( byte score)
    {
        float normalizedB = Mathf.Clamp01((float)score / Setings.maxScores);
        exponent = Mathf.Lerp( 0.5f, 0.01f, normalizedB);

        float curved = Mathf.Pow(normalizedB, exponent);
        spawnInterval = Mathf.Lerp(0.5f, 0.01f, curved);
    }
}