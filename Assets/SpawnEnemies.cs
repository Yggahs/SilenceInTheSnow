using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : Photon.MonoBehaviour {
    public GameObject spawnedEnemy;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    [PunRPC]
    void Awake()
    {
        InvokeRepeating("SpawnEnemy", spawnTime, spawnDelay);
    }
    
    void SpawnEnemy()
    {
        PhotonNetwork.Instantiate(spawnedEnemy.name, gameObject.transform.position, Quaternion.identity, 0);
        if (stopSpawning)
        {
            CancelInvoke("SpawnEnemy");
        }
    }
}
