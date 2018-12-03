using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class NetworkManager : Photon.MonoBehaviour
{   
    private const string roomName = "RoomName";
    private TypedLobby lobbyName = new TypedLobby("Canvas_of_white", LobbyType.Default);
    private RoomInfo[] roomsList;
    public GameObject player;
    public GameObject droplets;
    public GameObject ScoreUI;
    float timer = 0f;
    float endMatchTime = 300f;
    public Text TimerText;
    public Text WinnerText;
    
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v4.2");
        ScoreUI.SetActive(false); //sets scores inactive while not playing
        WinnerText.GetComponent<Text>().enabled = false;
        TimerText.GetComponent<Text>().enabled = false;
    }
    //create server / join room buttons
    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
            {
                PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 4, IsOpen = true, IsVisible = true }, lobbyName);
            }
            if (roomsList != null)
            {
                for (int i = 0; i < roomsList.Length; i++)
                {
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].Name))
                    {
                        PhotonNetwork.JoinRoom(roomsList[i].Name);
                    }
                }
            }
        }
    }
    void OnConnectedToMaster()
    {   
        PhotonNetwork.JoinLobby(lobbyName);
    }
    void OnReceivedRoomListUpdate()
    {
        Debug.Log("Room was created");
        roomsList = PhotonNetwork.GetRoomList();
 
    }    
    void OnJoinedLobby()
    {       
        Debug.Log("Joined Lobby");
        //sets cursor to invisible while playing
        if (!PhotonNetwork.inRoom)
        {
            Cursor.visible = true;     
        }
        else
        {
            Cursor.visible = false;
        }
        Cursor.lockState = CursorLockMode.None; //unlocks cursor to window while playing

    }
    void OnJoinedRoom()
    {
        
        Debug.Log("Connected to Room");
        PhotonNetwork.Instantiate(player.name, new Vector3(Random.Range(-50,50), 3,Random.Range(-50, 50)), Quaternion.identity, 0);
        Cursor.lockState = CursorLockMode.Locked; //locks cursor to window while playing
        ScoreUI.SetActive(true); // activates scores while playing
    }
    private void Update()
    {
        //if there are more than 2 players, start the timer
            if (PhotonNetwork.playerList.Length >= 2)
        {
            TimerText.GetComponent<Text>().enabled = true;
            //Debug.Log(PhotonNetwork.playerList.Length);
            EndMatch();
        }
    }
    //
    private void EndMatch()
    {
        // at the end of the time given, display winner; otherwise keep timer running
        if (timer >= endMatchTime)
        {
            timer += 0;
            Debug.Log("player "+ ScoreUI.GetComponent<Score>().highestplayer+1 + " wins with " + ScoreUI.GetComponent<Score>().max);
            string winnerUI = string.Concat("Player " + (ScoreUI.GetComponent<Score>().highestplayer + 1) + " wins with " + ScoreUI.GetComponent<Score>().max + "points");
            WinnerText.text = winnerUI;
            WinnerText.GetComponent<Text>().enabled = true;
            Invoke("Kickplayers",5f);
        }
        else
        {
            timer += Time.deltaTime;
            TimerText.text = timer.ToString();
        }
        string TimerUI = string.Concat(timer.ToString(), "/", endMatchTime.ToString());
        TimerText.text = TimerUI;        
    }
    void Kickplayers()
    {
        
        for (int i = 0; i <= 3; i++)
        {
            PhotonNetwork.CloseConnection(PhotonNetwork.playerList[i]); //kicks players after the match
        }
        //Destroyenemies();
        //Destroyplayers();
        Application.Quit();
    }
    ////destroys all players
    //void Destroyplayers()
    //{
    //    GameObject[] names = GameObject.FindGameObjectsWithTag("Player");

    //    foreach (GameObject item in names)
    //    {
    //        Destroy(item);
    //    }
    //}
    ////destroys all enemies
    //void Destroyenemies()
    //{
    //    GameObject[] names = GameObject.FindGameObjectsWithTag("Enemy");

    //    foreach (GameObject item in names)
    //    {
    //        Destroy(item);
    //    }
    //}
}

