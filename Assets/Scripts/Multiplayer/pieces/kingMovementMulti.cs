using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class kingMovementMulti : movementMulti, IPieceMovement
    {
		private bool didCastling = false;
		private nodeMulti[,] specialNodes;
		private pieceMulti[] rooks;

		private GcPlayerMulti p1;
		private GcPlayerMulti p2;
		private gridMulti grid;

		public kingMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player, piece)
		{
			BoundComputations += ComputeBound;
			specialNodes = new nodeMulti[2, 2];
			rooks = new pieceMulti[2];
			grid = GameManagerMulti.Instance.Grid;
			p1 = GameManagerMulti.Instance.P1;
			p2 = GameManagerMulti.Instance.P2;
		}

		public override void ComputeBound()
		{
			nodeMulti currNode = piece.Node;
			int origRow = currNode.row;
			int origCol = currNode.col;

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

			if (!moved && !didCastling && !player.IsChecked)
			{
				//check left
				int left = 1;
				bool freeLeft = true;
				int sign = GetSign();
				while (true)
				{
					int newCol = origCol - left * sign;
					if (newCol < 0 || newCol >= grid.Cols) break;
					nodeMulti toCheckNode = grid.GetNodeAt(origRow, newCol);

					if (toCheckNode.EmptySpace)
					{
						if (RulesMulti.IsGuardedMove(player, piece, toCheckNode))
						{
							freeLeft = false;
							break;
						}
					}
					else
					{
						pieceMulti cPiece = toCheckNode.Piece;
						if (RulesMulti.IsAlly(cPiece, piece) && cPiece.PieceTypeMulti == PieceTypeMulti.SQUARE)
						{
							rooks[0] = cPiece;
						}
						else
						{
							freeLeft = false;
						}
						break;
					}


					left++;
				}
				if (freeLeft && !rooks[0].IsMoved)
				{
					specialNodes[0, 0] = grid.GetNodeAt(origRow, origCol - 1 * sign); //for rook
					specialNodes[0, 1] = grid.GetNodeAt(origRow, origCol - 2 * sign); //for king
					ComputeMovePiece(specialNodes[0, 1]);
				}

				//check right
				int right = 1;
				bool freeRight = true;
				while (true)
				{
					int newCol = origCol + right * sign;
					if (newCol < 0 || newCol >= grid.Cols) break;
					nodeMulti toCheckNode = grid.GetNodeAt(origRow, newCol);

					if (toCheckNode.EmptySpace)
					{
						if (RulesMulti.IsGuardedMove(player, piece, toCheckNode))
						{
							freeRight = false;
							break;
						}
					}
					else
					{
						pieceMulti cPiece = toCheckNode.Piece;
						if (RulesMulti.IsAlly(cPiece, piece) && cPiece.PieceTypeMulti == PieceTypeMulti.SQUARE)
						{
							rooks[1] = cPiece;
						}
						else
						{
							freeRight = false;
						}
						break;
					}


					right++;
				}
				if (freeRight && !rooks[1].IsMoved)
				{
					specialNodes[1, 0] = grid.GetNodeAt(origRow, origCol + 1 * sign); //for rook
					specialNodes[1, 1] = grid.GetNodeAt(origRow, origCol + 2 * sign); //for king
					ComputeMovePiece(specialNodes[1, 1]);
				}

			}
		}

		int GetSign()
		{
			if (player == p1)
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}

		public override void Moved()
		{
			if (rooks[0] == null && rooks[1] == null) return;
			if (!moved)
			{
				moved = true;
				if (!didCastling)
				{
					if (specialNodes[0, 0] != null && piece.Node == specialNodes[0, 1])
					{
						rooks[0].MoveToXZ(specialNodes[0, 0], UpdateLeftRook);
						didCastling = true;
					}
					else if (specialNodes[1, 0] != null && piece.Node == specialNodes[1, 1])
					{
						rooks[1].MoveToXZ(specialNodes[1, 0], UpdateRightRook);
						didCastling = true;
					}
				}
			}
		}

		private void UpdateLeftRook()
		{
			rooks[0].UpdateNode(specialNodes[0, 0]);
		}

		private void UpdateRightRook()
		{
			rooks[1].UpdateNode(specialNodes[1, 0]);
		}
	}
}
