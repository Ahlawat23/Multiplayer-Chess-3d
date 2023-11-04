using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class circleMovementMulti : movementMulti, IPieceMovement
    {
		public circleMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player, piece)
		{
			BoundComputations += ComputeBound;
		}

		public override void ComputeBound()
		{
			Debug.Log("circleMOvement Computed");
			nodeMulti currNode = piece.Node;
			int origRow = currNode.row;
			int origCol = currNode.col;

			nodeMulti frontNode = null;
			nodeMulti leftEatNode = null;
			nodeMulti rightEatNode = null;

			gridMulti grid = GameManagerMulti.Instance.Grid;
			GcPlayerMulti p1 = p1 = GameManagerMulti.Instance.P1;

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

			ComputeMoveOrEatPiece(leftEatNode);
			ComputeMoveOrEatPiece(rightEatNode);
			ComputeMoveOrEatPiece(frontNode);
		}
	}
}
