using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class pawnMovementMulti : movementMulti, IPieceMovement
    {
		private bool didSpecialMove = false;
		private nodeMulti[] specialNodes = null;

		private GcPlayerMulti p1;
		private GcPlayerMulti p2;
		private gridMulti grid;

		public pawnMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player, piece)
		{
			BoundComputations += ComputeBound;
			GameManagerMulti.SwitchedEvent += OnSwitchEvent;
			specialNodes = new nodeMulti[2];

			p1 = GameManagerMulti.Instance.P1;
			p2 = GameManagerMulti.Instance.P2;
			grid = GameManagerMulti.Instance.Grid;
		}

		public override void ComputeBound()
		{
			nodeMulti currNode = piece.Node;
			int origRow = currNode.row;
			int origCol = currNode.col;

			nodeMulti frontNode = null;
			nodeMulti leftEatNode = null;
			nodeMulti rightEatNode = null;

			int toAdd = 0;
			if (p1.Has(piece))
			{
				toAdd = 1;
			}
			else
			{
				toAdd = -1;
			}

			frontNode = grid.GetNodeAt(origRow + toAdd, origCol);
			leftEatNode = grid.GetNodeAt(origRow + toAdd, origCol - 1);
			rightEatNode = grid.GetNodeAt(origRow + toAdd, origCol + 1);

			ComputeEatPiece(leftEatNode);
			ComputeEatPiece(rightEatNode);
			ComputeMovePiece(frontNode);

			if (!moved && !didSpecialMove)
			{
				if (frontNode.EmptySpace)
				{
					specialNodes[0] = frontNode;
					specialNodes[1] = grid.GetNodeAt(origRow + toAdd * 2, origCol);
					ComputeMovePiece(specialNodes[1]);
				}
			}
		}

		public void OnSwitchEvent()
		{
			if (!IsTurn()) return;
			if (specialNodes[0] == null || specialNodes[1] == null) return;

			if (moved && didSpecialMove)
			{ // on next move
				Debug.Log("released en passant");
				if (specialNodes[0] != null && specialNodes[0].Piece == piece)
				{
					specialNodes[0].Piece = null;
				}
				specialNodes[0] = null;
				specialNodes[1] = null;
			}
		}

		public override void Moved()
		{
			if (specialNodes[0] == null && specialNodes[1] == null) return;

			if (!moved)
			{
				moved = true;
				if (specialNodes[1] == piece.Node)
				{
					didSpecialMove = true;
					specialNodes[0].Piece = piece;
				}
			}
		}
	}

}