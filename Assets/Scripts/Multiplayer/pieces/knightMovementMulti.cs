using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class knightMovementMulti : movementMulti, IPieceMovement
    {
		public knightMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player, piece)
		{
			BoundComputations += ComputeBound;
		}

		public override void ComputeBound()
		{
			nodeMulti currNode = piece.Node;
			int origRow = currNode.row;
			int origCol = currNode.col;

			gridMulti grid = GameManagerMulti.Instance.Grid;

			for (int row = -2; row <= 2; row++)
			{
				if (row == 0) continue;
				int col = GetCol(row, true);
				int newRow = origRow + row;
				int newCol = origCol + col;
				nodeMulti newNode = grid.GetNodeAt(newRow, newCol);
				ComputeMoveOrEatPiece(newNode);
			}

			for (int row = -2; row <= 2; row++)
			{
				if (row == 0) continue;
				int col = GetCol(row, false);
				int newRow = origRow + row;
				int newCol = origCol + col;
				nodeMulti newNode = grid.GetNodeAt(newRow, newCol);
				ComputeMoveOrEatPiece(newNode);
			}

		}

		private int GetCol(int n, bool posSign)
		{
			if (Mathf.Abs(n) == 2) return 1 * ((posSign) ? 1 : -1);
			if (Mathf.Abs(n) == 1) return 2 * ((posSign) ? 1 : -1);
			return 0;
		}
	}
}
