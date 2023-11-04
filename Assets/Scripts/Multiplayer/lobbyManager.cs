using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.impactional.chess
{
    public class lobbyManager : MonoBehaviourPun, IPunOwnershipCallbacks
    {


        int viewIdCounter = 3;

        public Text[] userNameText;


        private void Start()
        {
            updateLobbyData();
        }

        public void updateLobbyData()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                userNameText[i].text = "player" + i.ToString();
            }
        }


        public void transferOwnership(GameObject piece, MultiPlayerType playerType)
        {

            piece.GetPhotonView().ViewID = viewIdCounter;
            viewIdCounter++;

            if (PhotonNetwork.IsMasterClient)
            {

                

               

                

                

                Debug.Log("after the id was : " + piece.GetPhotonView().ViewID);


                switch (playerType)
                {
                    case MultiPlayerType.white:
                        piece.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[0]);
                        break;
                    case MultiPlayerType.black:
                        piece.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[1]);
                        break;
                }
            }
        }

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            throw new System.NotImplementedException();
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}
