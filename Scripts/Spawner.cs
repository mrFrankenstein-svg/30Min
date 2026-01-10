using UnityEngine;
using UnityEngine.Events;

public class StoneSpawner : MonoBehaviour
{
    [Header("Префаб для спавна")]
    public GameObject prefab;

    [Header("Интервал между спавнами (в секундах)")]
    public float spawnInterval = 1.5f;
    [SerializeField] float exponent = 2;
    [SerializeField] float X = 0.15f;
    [SerializeField] float Y = 45f;


    private float spawnTimer = 0f;

    public static UnityEvent<byte> newObjectMaximumNumberCheck= new UnityEvent<byte>();

    private void Start()
    {
        ScoreCounter.OnTick+=SetSpawnTime;
        newObjectMaximumNumberCheck.AddListener(NewObjectMaximumNumberCheckVoid);
    }
    private void OnDestroy()
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

    void SpawnObject()
    {
        //float randomX = Random.Range(-25f, 25f);
        //Vector3 spawnPosition = new Vector3(randomX, X, Y);
        //GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);

        float randomX = Random.Range(-25f, 25f);
        Vector3 spawnPosition = new Vector3(randomX, X, Y);
        GameObject obj = null;
        if (prefab!=null)
            obj = ObjectPool.GetAnObject(prefab.name);
        else
        {
            Debug.LogError("StoneSpawner: prefab == null");
            return;
        }
            obj.transform.position = spawnPosition;
        obj.transform.localScale = new Vector3(0,0,0);
        obj.GetComponent<StoneSize>().sizeX = Random.Range(Setings.minStoneSize, Setings.maxStoneSize);
        obj.GetComponent<StoneSize>().sizeZ = Random.Range(Setings.minStoneSize, Setings.maxStoneSize);
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