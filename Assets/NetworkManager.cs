using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkManager : MonoBehaviour
{
    private const string roomName = "RoomName";
    private TypedLobby lobbyName = new TypedLobby("Ano_eerrretbytttttttttttt", LobbyType.Default);
    private RoomInfo[] roomsList;
    public GameObject player;
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
        roomsList = PhotonNetwork.GetRoomList();
    }
    void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
    void OnJoinedRoom()
    {
        Debug.Log("Connected to Room");
        PhotonNetwork.Instantiate(player.name, Vector3.up * 5, Quaternion.identity, 0);
    }
}

