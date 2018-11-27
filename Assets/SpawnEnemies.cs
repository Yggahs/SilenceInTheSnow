using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : Photon.MonoBehaviour {
    public GameObject spawnedEnemy;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public int enemycount = 0;
    [PunRPC]
    void Awake()
    {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnDelay);
    }
    
    void SpawnEnemy()
    {
        PhotonNetwork.Instantiate(spawnedEnemy.name, gameObject.transform.position, Quaternion.identity, 0);
        enemycount++;
        if (enemycount >= 10)
        {
            stopSpawning = true;
        }
        if (stopSpawning)
        {
            CancelInvoke("SpawnEnemy");
        }
    }
}
