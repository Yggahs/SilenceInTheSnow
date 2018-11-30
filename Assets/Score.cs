﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour {

    float player1Score;
    float player3Score, player4Score,player2Score;
    public GameObject p1, p2, p3, p4;
    int numbPlayer;
    public Text score1, score2, score3, score4;
    Vector4 scores; 

    // Use this for initialization
    void Start ()
    {
        gameObject.SetActive(false);

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

        numbPlayer = PhotonNetwork.room.PlayerCount;

        if (PhotonNetwork.inRoom == true)
        {
            gameObject.SetActive(true);
        }
        else gameObject.SetActive(false);

        if (numbPlayer == 1)
        {
            p1.SetActive(true);
            p2.SetActive(false);
            p3.SetActive(false);
            p4.SetActive(false);
        }else if (numbPlayer == 2)
        {
            p1.SetActive(true);
            p2.SetActive(true);
            p3.SetActive(false);
            p4.SetActive(false);
        } else if(numbPlayer == 3)
        {
            p1.SetActive(true);
            p2.SetActive(true);
            p3.SetActive(true);
            p4.SetActive(false);
        } else 
        {
            p1.SetActive(true);
            p2.SetActive(true);
            p3.SetActive(true);
            p4.SetActive(true);
        }
    }
}
