using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : Photon.MonoBehaviour {
    public GameObject spawnedEnemy;
    public bool stopSpawning = false;
    public float spawnTime = 5f;
    public float spawnDelay = 2.5f;
    public int enemycount = 0;
    //spawn enemies with a delay after 5 seconds
    [PunRPC]
    private void Awake()
    {
        
            InvokeRepeating("SpawnEnemy", spawnTime, spawnDelay);
        
    }
    //if there are more than 2 players, enemies will start spawning; if a spawner has spawned more than 10 enemis, they will stop spawning
    void SpawnEnemy()
    {
        if (PhotonNetwork.playerList.Length >= 1)
        {
            if (enemycount <= 9)
            {
            PhotonNetwork.Instantiate(spawnedEnemy.name, gameObject.transform.position, Quaternion.identity, 0);
            enemycount++;
            }
        }
    }
}
