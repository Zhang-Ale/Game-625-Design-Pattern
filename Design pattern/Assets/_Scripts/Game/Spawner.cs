using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public float minSpawnTime;
    public float maxSpawnTime;

    private void Start()
    {
        RandomSpawn(enemyPrefab);
    }
    private void RandomSpawn(GameObject Pref)
    {
        Instantiate(Pref, transform.position, Quaternion.identity);
        var spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("RandomSpawn", spawnTime);
    }

    public void QuickSpawn(GameObject Pref)
    {
        Instantiate(Pref, transform.position, Quaternion.identity);
    }

}
