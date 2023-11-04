using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

namespace com.impactional.chess
{
	public enum PieceTypeMulti
	{
		NONE,
		CIRCLE,
		TRIANGLE,
		SQUARE,
		HEXAGON,
		CROSS,
		RECTANGLE, //Sample
	}
	public class pieceMulti : moveableMulti, IClickable
    {
		[SerializeField]
		private PieceTypeMulti pieceTypeMulti;
		[SerializeField]
		private nodeMulti node;
		[SerializeField]
		private MovementTypeMulti movementType;

		private IPieceMovement pieceMovement;
		private bool dropping;

		private List<nodeMulti> possibleMoves;
		private List<nodeMulti> possibleEats;

		public IPieceMovement PieceMovement
		{
			get { return pieceMovement; }
			set
			{
				pieceMovement = value;
			}
		}

		/*
		private Piece check;

		public bool Checking {
			get {return check != null;}
		}

		public Piece Check {
			get {return check;}
			set {check = value;}
		}
		*/

		public bool IsMoved
		{
			get
			{
				if (pieceMovement != null)
				{
					return pieceMovement.IsMoved();
				}

				return false;
			}
		}

		public List<nodeMulti> PossibleMoves
		{
			get { return possibleMoves; }
		}

		public List<nodeMulti> PossibleEats
		{
			get { return possibleEats; }
		}

		public PieceTypeMulti PieceTypeMulti
		{
			get { return pieceTypeMulti; }
		}

		public MovementTypeMulti MovementType
		{
			get { return movementType; }
		}

		void Awake()
		{
			possibleEats = new List<nodeMulti>();
			possibleMoves = new List<nodeMulti>();
		}


		protected override void Start()
		{
			base.Start();
		}

		public void HighlightPossibleMoves()
		{
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				possibleMoves[i].HighlightMove();
			}
		}

		

		public void UnHighlightPossibleMoves()
		{
			for (int i = 0; i < possibleMoves.Count; i++)
			{
				possibleMoves[i].UnhighlightMove();
			}
		}

		public void HighlightPossibleEats()
		{

			for (int i = 0; i < possibleEats.Count; i++)
			{
				possibleEats[i].HighlightEat();
			}
		}

		

		public void UnHighlightPossibleEats()
		{
			for (int i = 0; i < possibleEats.Count; i++)
			{
				possibleEats[i].UnhighlightEat();
			}
		}

		public bool IsPossibleMove(nodeMulti node)
		{
			return this.possibleMoves.Contains(node);
		}

		public bool IsPossibleEat(nodeMulti node)
		{
			return this.possibleEats.Contains(node);
		}

		public void AddPossibleMoves(params nodeMulti[] nodes)
		{
			for (int i = 0; i < nodes.Length; i++)
			{
				this.possibleMoves.Add(nodes[i]);
			}
		}

		public void AddPossibleEats(params nodeMulti[] nodes)
		{
			for (int i = 0; i < nodes.Length; i++)
			{
				this.possibleEats.Add(nodes[i]);
			}
		}

		public void ClearPossibleMoves()
		{
			while (possibleMoves.Count > 0)
			{
				nodeMulti node = possibleMoves[0];
				possibleMoves.Remove(node);
			}
		}

		public void ClearPossibleEats()
		{
			while (possibleEats.Count > 0)
			{
				nodeMulti node = possibleEats[0];
				possibleEats.Remove(node);
			}
		}

		/*
			public void ClearCheck(GCPlayer player) {
				if (node != null) {
					node.UnhighlightCheck();
				}
				if (check != null) {
					check.Node.UnhighlightCheck();
					check = null;
				}
				if (player != null) {
					player.CheckedBy = null;
				}
			}
		*/

		public void Compute()
		{
            //photonView.RPC("ComputeRPC", RpcTarget.All);
            pieceMovement.Compute();
        }

		[PunRPC]
		 public void ComputeRPC()
        {
			pieceMovement.Compute();
		}

		public override void MoveToXZ(nodeMulti node, Action finishCallback)
		{
			base.MoveToXZ(node, finishCallback);
			pieceMovement.Moved();
		}

		[PunRPC]
		public void pieceMovementSetMovedRPC()
        {
			pieceMovement.Moved();
		}

		public string ChessCoords
		{
			get
			{
				if (node == null) return null;

				return node.ChessCoords;
			}
		}

		public nodeMulti Node
		{
			get { return node; }
		}

		

		public void UpdateNode(nodeMulti n)
        {
			if (node != null)
			{
				node.Clear();
			}
			node = n;
			if (node != null)
			{
				node.Piece = this;
			}


			//if (GameManagerMulti.Instance.IsReady)
			//{

			//	if (n != null)
			//	{
			//		photonView.RPC("nodeUpdateRPC", RpcTarget.AllBufferedViaServer, n.transform.position.x, n.transform.position.y, n.transform.position.z);
			//	}
			//	else
			//	{
			//		photonView.RPC("nullTheNodeVarRPC", RpcTarget.AllBufferedViaServer);
			//	}
			//}

			//else
			//{

			//}

		}

		[PunRPC]
		public void nodeUpdateRPC(float x, float y, float z)
        {
			Debug.Log("node got found");
			Vector3 newNodePos = new Vector3(x, y, z);
			nodeMulti newNode =  GameManagerMulti.Instance.Grid.GetNodeAt(newNodePos);

            if (node != null)
            {
                node.Clear();
            }
            node = newNode;
			if (node != null)
			{
				node.Piece = this;
			}

		}

		[PunRPC]
		public void nullTheNodeVarRPC()
        {
			Debug.Log("Node got null");
			node = null;
        }

		public bool Inform<T>(T arg)
		{
			//TODO
			return true;
		}

		public void Highlight()
		{
			SetEmission(GameManagerMulti.Instance.PieceHighlightColor);
		}

		

		public void ZeroGravity()
		{
			gameObject.GetComponent<Rigidbody>().useGravity = false;
		}


		public void Pickup()
		{
			Highlight();
			ZeroGravity();
			MoveBy(new Vector3(0, 1, 0), null);
		}


		
		

		
		



		public void Drop()
		{
			//photonView.RPC("dropRpc", RpcTarget.AllBufferedViaServer);

			dropping = true;
			SetEmissionOriginal();
			StopMoveCoroutine();
			gameObject.GetComponent<Rigidbody>().useGravity = true;
			//GCPlayer currPlayer = GameManager.Instance.CurrentPlayer;
			UnHighlightPossibleEats();
			UnHighlightPossibleMoves();
			ready = false;

			GameManagerMulti.Instance.GameState.Cancel();
		}

		[PunRPC]
		public void dropRpc()
        {
			dropping = true;
			SetEmissionOriginal();
			StopMoveCoroutine();
			gameObject.GetComponent<Rigidbody>().useGravity = true;
			//GCPlayer currPlayer = GameManager.Instance.CurrentPlayer;
			UnHighlightPossibleEats();
			UnHighlightPossibleMoves();
			ready = false;

			GameManagerMulti.Instance.GameState.Cancel();
		}

		
		

		public bool isItMine()
        {

            if (photonView.IsMine)
                return true;
            else return false;
        }

        //EXPERIMENT
        void OnCollisionEnter(Collision collision)
		{
			if (dropping && collision.collider.gameObject)
			{
				ready = true;
				dropping = false;
			}
		}

		public void GrabPiece()
        {
			if (photonView.IsMine)
				photonView.RPC("GrabPieceRPC", RpcTarget.AllBufferedViaServer);
			else return;
        }

	   [PunRPC]
	   public void GrabPieceRPC()
        {
			Pickup();
			Compute();
			HighlightPossibleMoves();
			HighlightPossibleEats();
			GameManagerMulti.Instance.GameState.Grab();
		}
	}
}
