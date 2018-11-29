using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour {

    float player1Score;
    float player3Score, player4Score,player2Score;
    public Text score1, score2, score3, score4;
    Vector4 scores; 

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {

        //player1Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.y;
        //player2Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.w;
        //player3Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.z;
        //player4Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.x;
        scores = SplatManagerSystem.instance.scores;

        player1Score = (int)(scores.y*5120);
        player2Score = (int)(scores.w * 5120);
        player3Score = (int)(scores.z * 5120);
        player4Score = (int)(scores.x * 5120);

        score1.text = player1Score.ToString();
        score2.text = player2Score.ToString();
        score3.text = player3Score.ToString();
        score4.text = player4Score.ToString();

    }
}
