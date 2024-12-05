using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {
    public static List<PoolObjectInfo> ObjectInfos = new List<PoolObjectInfo>();
    public static GameObject SpawnObject(GameObject objectToSpwan, Vector3 spawPosition, Quaternion spawQuaternion){
        PoolObjectInfo pool = ObjectInfos.Find(p => p.LookupString == objectToSpwan.name);
        if(pool == null){
            pool = new PoolObjectInfo(){
                LookupString = objectToSpwan.name
            };
        }
        ObjectInfos.Add(pool);
        GameObject spawableObj = pool.InactiveObjects.FirstOrDefault();
        if(spawableObj == null){
            spawableObj = Instantiate(objectToSpwan, spawPosition, spawQuaternion);
        }
        else{
            spawableObj.transform.position = spawPosition;
            spawableObj.transform.rotation = spawQuaternion;
            pool.InactiveObjects.Remove(spawableObj);
            spawableObj.SetActive(true);
        }
        return spawableObj;
    }
    public static void ReturnObjectToPool(GameObject obj){
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PoolObjectInfo pool = ObjectInfos.Find(p => p.LookupString == goName);
        if(pool == null){
            Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
        }
        else{
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}

public class PoolObjectInfo{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}