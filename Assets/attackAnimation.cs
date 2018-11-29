using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAnimation : MonoBehaviour {
    
    public void attack()
    {
        GetComponent<Animation>().Play();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Enemy(Clone)")
        {
            Debug.Log(gameObject.transform.parent.GetComponent<Player>().playerIDinPlayer);
            collision.gameObject.GetComponent<AggroPlayers>().playerIDinEnemy = gameObject.transform.parent.GetComponent<Player>().playerIDinPlayer;
        }
    }
}
