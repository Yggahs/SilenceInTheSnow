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
        //player1Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.y;
        //player2Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.w;
        //player3Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.z;
        //player4Score = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>().scores.x;

        //scores are based on the amount of space that the splatters occupy, so they need to be multiplied by 5120 to have a significant score

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
        GetHigherScore();
        
    }

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
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    //if (stream.isWriting)
    //    //{
    //    //    stream.SendNext(p1);
    //    //    stream.SendNext(p2);
    //    //    stream.SendNext(p3);
    //    //    stream.SendNext(p4);
    //    //}
    //    //else
    //    //{
    //    //    this.p1 = (GameObject)stream.ReceiveNext();
    //    //    this.p2 = (GameObject)stream.ReceiveNext();
    //    //    this.p3 = (GameObject)stream.ReceiveNext();
    //    //    this.p4 = (GameObject)stream.ReceiveNext();
    //    //}
    //}
}
