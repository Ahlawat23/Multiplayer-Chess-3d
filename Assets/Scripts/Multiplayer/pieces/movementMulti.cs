using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{

    public enum MovementTypeMulti
    {
        NONE = 0,
        KING = 1,
        PAWN = 2,
        ROOK = 3,
        BISHOP = 4,
        QUEEN = 5,
        KNIGHT = 6,
        CIRCLE = 7,
        CROSS = 8,
    }

    public class movementMulti : ScriptableObject
    {
		protected GcPlayerMulti player;
		protected pieceMulti piece;
		protected bool moved = false;

		public event ComputeBound BoundComputations;

		public movementMulti(GcPlayerMulti player, pieceMulti piece)
		{
			this.player = player;
			this.piece = piece;
			BoundComputations += ClearPossibles;
		}

		void DisableCalculation()
		{
			BoundComputations = null;
		}

		void OnDisable()
		{
			DisableCalculation();
		}

		public virtual void ComputeBound() { }

		public bool IsTurn()
		{
			if (player == GameManagerMulti.Instance.CurrentPlayer)
			{
				return true;
			}

			return false;
		}

		public void ClearPossibles()
		{
			piece.ClearPossibleEats();
			piece.ClearPossibleMoves();
		}

		public bool ComputeMovePiece(nodeMulti toCheckNode)
		{
			if (toCheckNode == null) return false;
			if (toCheckNode.EmptySpace)
			{
				piece.AddPossibleMoves(toCheckNode);
				return true;
			}

			return false;
		}

		public bool ComputeEatPiece(nodeMulti toCheckNode)
		{
			if (toCheckNode == null) return false;
			if (!toCheckNode.EmptySpace && RulesMulti.IsEnemy(piece, toCheckNode.Piece))
			{
				AddToCheckOrEat(toCheckNode);
				return true;
			}

			return false;
		}

		public bool ComputeMoveOrEatPiece(nodeMulti toCheckNode)
		{
			if (toCheckNode == null) return false;
			if (toCheckNode.EmptySpace)
			{
				piece.AddPossibleMoves(toCheckNode);
				return true;
			}
			else if (RulesMulti.IsEnemy(piece, toCheckNode.Piece))
			{
				AddToCheckOrEat(toCheckNode);
				return true;
			}

			return false;
		}

		public virtual void Compute()
		{
			if (piece == null || piece.Node == null) return;
			BoundComputations();
		}

		//returns true if met an ally or enemy, this is for square and triangle, to cause a block
		public bool ComputeMoveOrEatPieceEnemyAlly(nodeMulti toCheckNode)
		{
			if (toCheckNode == null) return false;
			if (toCheckNode.EmptySpace)
			{
				piece.AddPossibleMoves(toCheckNode);
			}
			else if (RulesMulti.IsEnemy(piece, toCheckNode.Piece))
			{
				AddToCheckOrEat(toCheckNode);
				if (toCheckNode != toCheckNode.Piece.Node)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return true;
			}

			return false;
		}

		private void AddToCheckOrEat(nodeMulti toCheckNode)
		{
			if (RulesMulti.CheckKing(player, piece.Node, toCheckNode))
			{
				//playerPiece.Check = toCheckPiece;
			}
			else
			{
				piece.AddPossibleEats(toCheckNode);
			}
		}

		public bool IsMoved()
		{
			return moved;
		}

		public virtual void Moved()
		{
			if (!moved)
			{
				moved = true;
			}

			Debug.Log("MOVED: " + moved);
		}
	}
}
