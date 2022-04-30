using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;
    
    void Awake() {
        Instance = this;
        
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools) {
            Queue<GameObject> objPool = new Queue<GameObject>();
        
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
        
            poolDict.Add(pool.tag, objPool);
        }
    }


    public GameObject SpawnFromPool(string poolTag, Vector3 pos, Quaternion rot) {
        if (!poolDict.ContainsKey(poolTag)) {
            throw new Exception("Pool with tag " + poolTag + " does not exist.");
        }
        
        GameObject objToSpawn = poolDict[poolTag].Dequeue();

        objToSpawn.SetActive(true);
        if (poolTag == "coins" || poolTag == "goldcoins") {
            objToSpawn.GetComponentsInChildren<MeshRenderer>()
                .Where(mr => mr.name == "coin" || mr.name == "goldcoin").ToList()
                .ForEach(mr => mr.enabled = true);
        }
        objToSpawn.transform.position = pos;
        objToSpawn.transform.rotation = rot;

        poolDict[poolTag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}