using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs = new List<GameObject>();
    [SerializeField] List<GameObject> myObjects = new List<GameObject>();
    [SerializeField] private static ObjectPool thisObjectPool;
    private void Awake()
    {
        gameObject.name = "ObjPool";
        thisObjectPool = this;
    }


    public static GameObject GetAnObject(string name)
    {

        GameObject obj = null;  //создаЄм пустышку дл€ работы
        if (thisObjectPool.myObjects.Count != 0)    //если в хабе есть обьекты
            //obj = myObjects.First(x => x.name == name);
            obj = thisObjectPool.myObjects.FindLast    //ищем последний с подход€щим именем
                (
                    delegate (GameObject myObj)
                    {
                        return myObj.name == name;
                    }
                );

        if (obj != null)    //если обьект найден
        {
            thisObjectPool.myObjects.Remove(obj);
            obj.transform.parent = null;
            obj.SetActive(true);
            return obj;
        }
        else   //если подход€щих обьектов не найдено
        {
            GameObject findPrefabObj = thisObjectPool.prefabs.First(x => x.name == name);

            if (findPrefabObj != null)
            {
                GameObject instantiateObj = Instantiate(findPrefabObj);
                char[] MyChar = { '(', 'C', 'l', 'o', 'n', 'e', ')' };
                instantiateObj.name = instantiateObj.name.TrimEnd(MyChar);
                return instantiateObj;
            }
            else
            {
                Debug.LogError("ObjectPoo: there are no objects with this name \"" + name + "\"");
                return null;
            }
        }
    }
    public static void GiveAwayTheObject(GameObject hydedGameObject)
    {
        List<GameObject> obj = thisObjectPool.myObjects.FindAll    //ищем все обьекты с именем как у передаваемого обьекта
            (
                delegate (GameObject myObj)
                {
                    return myObj.name == hydedGameObject.name;
                }
            );

        if (obj.Count > 0) //если количество найденных обьектов больше 0
        {
            if (obj.Count >= 250)   //если количество найденных обьектов ЅќЋ№Ў≈ максимально допустимого
            {
                hydedGameObject.SetActive(false);
                Destroy(hydedGameObject);

                for (int i = 0; i < obj.Count - 250; i++)  //удал€ем лишние обьекты 
                {
                    GameObject excessObj = thisObjectPool.myObjects.Last();
                    thisObjectPool.myObjects.Remove(excessObj);
                    Destroy(excessObj);
                }
            }
            else
            {
                hydedGameObject.SetActive(false);
                thisObjectPool.myObjects.Add(hydedGameObject);
                hydedGameObject.transform.position = new Vector3(0,0,0);
                hydedGameObject.transform.parent = thisObjectPool.gameObject.transform;
            }
        }
        else
        {
            thisObjectPool.myObjects.Add(hydedGameObject);
            hydedGameObject.transform.position = new Vector3(0, 0, 0);
            hydedGameObject.transform.parent = thisObjectPool.gameObject.transform;
            hydedGameObject.SetActive(false);
        }
    }
}
