using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]

public class StoneSpawnerSlave : ScriptableObject, IStoneSpawnerSlave
{
    public GameObject prefab { get { return pref; } }
    [SerializeField] private GameObject pref;
    public int chanceOfPrefab { get { return chance; } }
    [SerializeField] private int chance;

    public bool needToSizeChange { get { return needToSizeChang; } }
    [SerializeField] private bool needToSizeChang =false;
}
//public class StoneSpawnerSlave : ScriptableObject, IStoneSpawnerSlave
//{
//    public  GameObject prefab { get; }
//    public  int chanceOfPrefab { get; }
//}

//[CreateAssetMenu(menuName = "ScriptableObject")]
//public class BasicStoneSlave : StoneSpawnerSlave
//{
//    [SerializeField] private GameObject pref;
//    [SerializeField] private int chance;

//    public override GameObject prefab => pref;
//    public override int chanceOfPrefab => chance;
//}