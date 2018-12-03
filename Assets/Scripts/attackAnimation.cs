using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour {
    //play animation 'attack01'
    public void Attack()
    {
        GetComponent<Animation>().Play();
    }

    // if the sword hits an enemy, send the player's id to the enemy hit
    private void OnTriggerEnter(Collider collision)
    {
            if (collision.gameObject.name == "Enemy(Clone)")
            {
                collision.gameObject.GetComponent<AggroPlayers>().playerIDinEnemy = gameObject.transform.parent.GetComponent<Player>().playerIDinPlayer;
            }
    }
}
