using UnityEngine;
using System.Collections;

public class Menu : Photon.MonoBehaviour {

	//public Texture2D menu;
	//public Texture2D sliderYellow;
	public Texture2D sliderRed;
	//public Texture2D sliderGreen;
	public Texture2D sliderBlue;

    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    
    [PunRPC]
	void OnGUI ()
    {
        
        Vector4 scores = SplatManagerSystem.instance.scores + new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
        int redScore = (int)(512 * (scores.y /*/ totalScores*/ ));
        int blueScore = (int)(512 * (scores.w /*/ totalScores)*/));
        //int yellowScore = (int)( 512 * ( scores.x /*/ totalScores )*/ ));
        //int greenScore = (int)( 512 * ( scores.z /*/ totalScores )*/ ));
        //GUI.DrawTexture (new Rect (20, 20, menu.width, menu.height), menu);


        //float totalScores = scores.x + scores.y + scores.z + scores.w;



        //GUI.DrawTexture (new Rect (20 + menu.width + 20, 20, yelowScore, 30), sliderYellow);
            GUI.DrawTexture(new Rect(0, 20, redScore, 30), sliderRed);
            GUI.DrawTexture(new Rect(0, 60, blueScore, 30), sliderBlue);
            //GUI.DrawTexture(new Rect(0, 100, blueScore, 30), sliderGreen);
            //GUI.DrawTexture(new Rect(0, 140, blueScore, 30), sliderYellow);
        //GUI.DrawTexture (new Rect (20 + menu.width + 20, 100, greenScore, 30), sliderGreen);
        //GUI.DrawTexture (new Rect (20 + menu.width + 20, 140, blueScore, 30), sliderBlue);



    }
	}

