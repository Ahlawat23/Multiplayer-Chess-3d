using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace com.impactional.chess
{
	public enum MultiPlayerType
    {
		white,
		black
    }

    public class GcPlayerMulti : IClicker 
    {
		private MultiPlayerType type;

		private List<pieceMulti> pieces;
		private List<pieceMulti> eatenPieces;

		public pieceMulti piece;
		private pieceMulti checkedBy; //Experimental

		//Experimental
		public bool IsChecked
		{
			get { return checkedBy != null; }
		}

		public List<pieceMulti> Pieces
		{
			get { return pieces; }
		}

		public List<pieceMulti> EatenPieces
		{
			get { return eatenPieces; }
		}

		//Experimental
		public pieceMulti CheckedBy
		{
			get { return checkedBy; }
			set
			{
				checkedBy = value;
			}
		}

		public pieceMulti HoldingPiece
		{
			get { return piece; }
		}

		public bool IsReady
		{
			get
			{
				for (int i = 0; i < pieces.Count; i++)
				{
					if (!pieces[i].IsReady) return false;
				}

				return true;
			}
		}

		public MultiPlayerType Type
		{
			get { return type; }
		}

		public GcPlayerMulti(MultiPlayerType type)
		{
			this.type = type;
			pieces = new List<pieceMulti>();
			eatenPieces = new List<pieceMulti>();
		}

		public void EnableInput()
		{
			inputManagerMulti.InputEvent += OnInputEvent;
		}

		public void DisableInput()
		{
			inputManagerMulti.InputEvent -= OnInputEvent;
		}

		void OnDisable()
		{
			DisableInput();
		}

		public void OnInputEvent(InputActionTypeMulti action)
		{
			switch (action)
			{
				case InputActionTypeMulti.GRAB_PIECE:
					nodeMulti gNode  = finderMulti.RayHitFromScreen<nodeMulti>(Input.mousePosition);
					if (gNode == null) break;
					Debug.Log(gNode.name + "has been selected");
                    GameManagerMulti.Instance.GrabThePieceGameMan(gNode);

                    //check clickable for tile and piece then pass Player
                    //check if player has piece - PIECE 
                    //check if player has piece if not empty - NODE 
                    break;
                case InputActionTypeMulti.CANCEL_PIECE:
					if (piece != null)
					{
						//if (!piece.IsReady) break;
						piece.Drop();
						piece = null;
						GameManager.Instance.GameState.Cancel();
					}
					break;
				case InputActionTypeMulti.PLACE_PIECE:
					nodeMulti tNode = finderMulti.RayHitFromScreen<nodeMulti>(Input.mousePosition);
					if (tNode == null) break;
					GameManagerMulti.Instance.PlaceThePieceGameMan(tNode);
					break;
			}



		}

		
		public void GrabThePiece(float x, float y, float z)
        {
			Debug.Log("node got found");
			Vector3 newNodePos = new Vector3(x, y, z);
			nodeMulti gNode = GameManagerMulti.Instance.Grid.GetNodeAt(newNodePos);

			
			piece = gNode.Piece;
			if (piece == null) return;
			if (!piece.IsReady) return;
			if (Click(gNode) && piece && Has(piece) && Click(piece))
			{

				piece.Pickup();
				piece.Compute();
				piece.HighlightPossibleMoves();
				piece.HighlightPossibleEats();
				GameManagerMulti.Instance.GameState.Grab();

			}
		}

	
		

	
		public void PlaceThePiec(float x, float y, float z)
        {
			Debug.Log("node got found");
			Vector3 newNodePos = new Vector3(x, y, z);
			nodeMulti tNode = GameManagerMulti.Instance.Grid.GetNodeAt(newNodePos);

			if (tNode == null) return;
			pieceMulti tPiece = tNode.Piece;
			if (tPiece == null)
			{
				if (piece.IsPossibleMove(tNode))
				{
					if (RulesMulti.IsCheckMove(this, piece, tNode, true))
					{
						Debug.Log("Move checked"); // do nothing
					}
					else
					{
						piece.MoveToXZ(tNode, Drop);
                        GameManagerMulti.Instance.GameState.Place();
                        //GameManagerMulti.Instance.PlaceThePieceGameMan();
                    }
				}
			}
			else
			{
				if (piece.IsPossibleEat(tNode))
				{
					if (RulesMulti.IsCheckEat(this, piece, tNode, true))
					{
						Debug.Log("Eat checked"); // do nothing
					}
					else
					{
						GcPlayerMulti oppPlayer = GameManagerMulti.Instance.Opponent(this);
						oppPlayer.RemovePiece(tPiece);
						AddEatenPieces(tPiece);
						tPiece.ScaleOut(0.2f, 1.5f);
						piece.MoveToXZ(tNode, Drop);
                        GameManagerMulti.Instance.GameState.Place();
                        //GameManagerMulti.Instance.PlaceThePieceGameMan();
                    }
				}
			}
		}
		
		
	

		public void ClearPiecesPossibles()
		{
			for (int i = 0; i < pieces.Count; i++)
			{
				pieces[i].ClearPossibleEats();
				pieces[i].ClearPossibleMoves();
			}
		}

		public void ClearCheck()
		{
			if (checkedBy == null) return;
			checkedBy = null;
			//checkedBy.ClearCheck(this);
		}

		//the methods inside must be in order
		private void Drop()
		{
			piece.Drop();
			piece.Compute();
            GameManagerMulti.Instance.GameState.Release();
            piece = null;
		}

		public bool Has(pieceMulti piece)
		{
			return pieces.Contains(piece);
		}

		public bool Click(IClickable clickable)
		{
			if (clickable == null) return false;
			return clickable.Inform<GcPlayerMulti>(this);
		}

		public void AddPieces(params pieceMulti[] pieces)
		{
			for (int i = 0; i < pieces.Length; i++)
			{
				this.pieces.Add(pieces[i]);
			}
		}

		public void AddEatenPieces(params pieceMulti[] pieces)
		{
			for (int i = 0; i < pieces.Length; i++)
			{
				this.eatenPieces.Add(pieces[i]);
			}
		}

		public bool RemovePiece(pieceMulti piece)
		{
			return pieces.Remove(piece);
		}

		public void ComputePieces()
		{ 
			for (int i = 0; i < pieces.Count; i++)
			{
				pieces[i].Compute();
			}
		}
	}
}
