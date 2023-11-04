using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class rookMovementMulti : movementMulti, IPieceMovement
    {
		public rookMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player,piece) {
			BoundComputations += ComputeBound;
		}

		public override void ComputeBound()
		{
			nodeMulti currNode = piece.Node;
			int origRow = currNode.row;
			int origCol = currNode.col;

			gridMulti grid = GameManagerMulti.Instance.Grid;
			//up
			for (int up = 1; up + origRow < grid.Rows; up++)
			{
				int newRow = up + origRow;
				nodeMulti newNode = grid.GetNodeAt(newRow, origCol);
				if (ComputeMoveOrEatPieceEnemyAlly(newNode)) break;
			}

			//left
			for (int left = -1; left + origCol >= 0; left--)
			{
				int newCol = left + origCol;
				nodeMulti newNode = grid.GetNodeAt(origRow, newCol);
				if (ComputeMoveOrEatPieceEnemyAlly(newNode)) break;
			}

			//right
			for (int right = 1; right + origCol < grid.Cols; right++)
			{
				int newCol = right + origCol;
				nodeMulti newNode = grid.GetNodeAt(origRow, newCol);
				if (ComputeMoveOrEatPieceEnemyAlly(newNode)) break;
			}

			//down
			for (int bot = -1; bot + origRow >= 0; bot--)
			{
				int newRow = bot + origRow;
				nodeMulti newNode = grid.GetNodeAt(newRow, origCol);
				if (ComputeMoveOrEatPieceEnemyAlly(newNode)) break;
			}
		}
	}
}
