using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour/*,IPunObservable*/ {

    float player1Score, player3Score, player4Score, player2Score;
    public GameObject p1, p2, p3, p4;
    int numbPlayer;
    public Text score1, score2, score3, score4;
    Vector4 scores;
    float[] ScoreArray = new float [4];
    public int max = 0;
    public int highestplayer = 0;
    
    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        if (PhotonNetwork.inRoom == true)
        {
            numbPlayer = PhotonNetwork.room.PlayerCount;
        }

        //scores are based on the amount of space that the splatters occupy, 
        //so they need to be multiplied by 5120 to have a significant score

        scores = SplatManagerSystem.instance.scores;

        player1Score = (int)(scores.y * 5120);
        player2Score = (int)(scores.w * 5120);
        player3Score = (int)(scores.z * 5120);
        player4Score = (int)(scores.x * 5120);

        //have scores appear on the ui

        score1.text = player1Score.ToString();
        score2.text = player2Score.ToString();
        score3.text = player3Score.ToString();
        score4.text = player4Score.ToString();



        //activate a score ui every time a player joins
        switch (numbPlayer)
        {
            case 4:
                p1.SetActive(true);
                p2.SetActive(true);
                p3.SetActive(true);
                p4.SetActive(true);
                break;
            case 3:
                p1.SetActive(true);
                p2.SetActive(true);
                p3.SetActive(true);
                p4.SetActive(false);
                break;
            case 2:
                p1.SetActive(true);
                p2.SetActive(true);
                p3.SetActive(false);
                p4.SetActive(false);
                break;
            case 1:
                p1.SetActive(true);
                p2.SetActive(false);
                p3.SetActive(false);
                p4.SetActive(false);
                break;
            default:
                p1.SetActive(false);
                p2.SetActive(false);
                p3.SetActive(false);
                p4.SetActive(false);
                break;
        }
        GetHigherScore();
        
    }
    //get the highest score and the player who owns it
    public void GetHigherScore()
    {
        ScoreArray[0] = player1Score;
        ScoreArray[1] = player2Score;
        ScoreArray[2] = player3Score;
        ScoreArray[3] = player4Score;

        for (int i = 0; i < ScoreArray.Length; i++)
        {
            if (ScoreArray[i] > max)
            {
                highestplayer = i;
                max = (int)ScoreArray[i];
            }
        }

    }
    
}
