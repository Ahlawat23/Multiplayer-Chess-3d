using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class crossMovementMulti : movementMulti, IPieceMovement
    {

		public crossMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player, piece)
		{
			BoundComputations += ComputeBound;
		}

		public override void ComputeBound()
		{
			nodeMulti currNode = piece.Node;
			int origRow = currNode.row;
			int origCol = currNode.col;

			gridMulti grid = GameManagerMulti.Instance.Grid;

			for (int row = -1; row <= 1; row++)
			{
				for (int col = -1; col <= 1; col++)
				{
					if (row == 0 && col == 0) continue;

					int newRow = origRow + row;
					int newCol = origCol + col;
					ComputeMoveOrEatPiece(grid.GetNodeAt(newRow, newCol));
				}
			}
		}
	}
}
