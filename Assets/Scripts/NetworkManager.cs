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
    float endMatchTime = 600f;
    public Text text;
    
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v4.2");
        ScoreUI.SetActive(false); //sets scores inactive while not playing
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
            //wincondition here
        }
        else
        {
            timer += Time.deltaTime;
            text.text = timer.ToString();
        }
        string TimerUI = string.Concat(timer.ToString(), "/", endMatchTime.ToString());
        text.text = TimerUI;        
    }
}

