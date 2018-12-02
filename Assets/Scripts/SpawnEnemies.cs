using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : Photon.MonoBehaviour {
    public GameObject spawnedEnemy;
    public bool stopSpawning = false;
    public float spawnTime = 5f;
    public float spawnDelay = 2.5f;
    public int enemycount = 0;
    [PunRPC]
    private void Awake()
    {
        
            InvokeRepeating("SpawnEnemy", spawnTime, spawnDelay);
        
    }
    
    void SpawnEnemy()
    {
        if (PhotonNetwork.playerList.Length >= 2)
        {
            if (enemycount <= 9)
            {
            PhotonNetwork.Instantiate(spawnedEnemy.name, gameObject.transform.position, Quaternion.identity, 0);
            enemycount++;
            }
        }
    }
}
