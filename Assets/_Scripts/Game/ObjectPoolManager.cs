using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookupstring == objectToSpawn.name);
        if (pool == null) // Pool does not exist, create it
        {
            pool = new PooledObjectInfo() { lookupstring = objectToSpawn.name };
            objectPools.Add(pool);
        }
        // Check if there are any inactive objects in the pool
        GameObject spawnableObj = pool.inactiveObjects.FirstOrDefault();
        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.inactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }
    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7); // By taking off 7, we remove the clone from the name of the passed in obj
        PooledObjectInfo pool = objectPools.Find(p => p.lookupstring == obj.name);
        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled");
        }
        else
        {
            obj.SetActive(false);
            pool.inactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public string lookupstring;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}
