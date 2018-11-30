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
    float endMatchTime = 600f; //600f for 10 mins, 60f for 1
    public Text text;
    
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v4.2");
    }
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
        ScoreUI.SetActive(false);
        roomsList = PhotonNetwork.GetRoomList();

    }
    
    void OnJoinedLobby()
    {       
        Debug.Log("Joined Lobby");
        if (PhotonNetwork.inRoom == false)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
            ScoreUI.SetActive(true);
        }
    }
    void OnJoinedRoom()
    {
        Debug.Log("Connected to Room");
        PhotonNetwork.Instantiate(player.name, Vector3.up * 5, Quaternion.identity, 0);
        Cursor.lockState = CursorLockMode.Locked; 
        
    }
    private void Update()
    {
            if (PhotonNetwork.playerList.Length >= 2)
        {
            Debug.Log(PhotonNetwork.playerList.Length);
            EndMatch();
        }
    }
    private void EndMatch()
    {

        if (timer == endMatchTime)
        {
            timer += 0;
            

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

