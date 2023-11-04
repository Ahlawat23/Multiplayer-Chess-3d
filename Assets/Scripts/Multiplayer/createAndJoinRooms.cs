using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

namespace com.impactional.chess
{
    public class createAndJoinRooms : MonoBehaviourPunCallbacks
    {
        string gameVersion = "1";
        [Header("Input Field For Creating Private Room")]
       
        [SerializeField] private InputField joinFeild;

        public byte maxPlayerPerRoom = 4;

        public string levelToLoad;

        



        [Header("debug:")]
        public Text debugText;
        public string roomCode;



        TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);


        

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

        }

        private void Start()
        {
           

           
            connect();



        }



        public void connect()
        {

            if (PhotonNetwork.IsConnected)
            {

                Debug.Log(PhotonNetwork.CurrentLobby.Name + " is the lobby and  the room is " + PhotonNetwork.CurrentRoom.Name);
            }
            else
            {

                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
           
        }



        public override void OnConnectedToMaster()
        {
            //have to wait for this message in order to use the create button
            Debug.Log("the server has made or connected, now we can create room");

        }




        //playerwithfriends create room
        public void joinLobby()
        {
            PhotonNetwork.JoinLobby(customLobby);
        }

        public void createRoom()
        {
           
            

            roomCode = gernrateRandomRoomCode();


            Debug.Log("The room code is :" + roomCode); 
            PhotonNetwork.CreateRoom("1234");



        }


        //genrated a random 4 alphabets code for the room name
        private string gernrateRandomRoomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] stringchars = new char[4];



            for (int i = 0; i < stringchars.Length; i++)
            {
                stringchars[i] = chars[Random.Range(0, chars.Length - 1)];
                //Debug.Log(i);
                //Debug.Log(chars[i]);

            }

            string finalRoomCode = new string(stringchars);

            return finalRoomCode;
        }




        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            debugText.text = message + " " + returnCode.ToString();

            Debug.Log(returnCode.ToString());
        }




        //playewithFriends join Room
        public void joinRoom()
        {
            
            PhotonNetwork.JoinRoom(joinFeild.text);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            debugText.text = message + " " + returnCode.ToString();

            Debug.Log(returnCode.ToString());
        }


        

        //takes you to the other 
        public override void OnJoinedRoom()
        {
            //meaning that you have created or joined room;

            PhotonNetwork.LoadLevel(levelToLoad);

        }
    }
}
