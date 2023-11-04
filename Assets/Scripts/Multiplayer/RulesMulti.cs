using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class RulesMulti : MonoBehaviour
    {
		public static bool IsAlly(pieceMulti piece1, pieceMulti piece2)
		{
			GcPlayerMulti p1 = GameManagerMulti.Instance.P1;
			if (p1.Has(piece1) && p1.Has(piece2)) return true;

			GcPlayerMulti p2 = GameManagerMulti.Instance.P2;
			if (p2.Has(piece1) && p2.Has(piece2)) return true;

			return false;
		}

		public static bool IsEnemy(pieceMulti piece1, pieceMulti piece2)
		{
			GcPlayerMulti p1 = GameManagerMulti.Instance.P1;
			if (p1.Has(piece1) && !p1.Has(piece2)) return true;
			if (p1.Has(piece2) && !p1.Has(piece1)) return true;
			return false;
		}

		public static bool CheckKing(GcPlayerMulti player, nodeMulti checkedByNode, nodeMulti checkedNode)
		{
			if (checkedNode.Piece.PieceTypeMulti == PieceTypeMulti.CROSS)
			{
				GameManagerMulti.Instance.Opponent(player).CheckedBy = checkedByNode.Piece;
				//checkedPiece.Node.HighlightCheck(); //Experimental
				//checkedBy.Node.HighlightCheck(); //Experimental
				return true;
			}
			return false;
		}

		//Modifies the move if modify = true
		//not safe
		public static bool IsCheckMove(GcPlayerMulti player, pieceMulti piece, nodeMulti tNode, bool modify)
		{
			nodeMulti oldNode = piece.Node;
			piece.UpdateNode(tNode);
			pieceMulti checkedBy = player.CheckedBy;
			player.ClearCheck();
			GameManagerMulti.Instance.Opponent(player).ComputePieces();
			if (player.IsChecked)
			{
				piece.UpdateNode(oldNode);
				player.CheckedBy = checkedBy;
				return true;
			}

			if (!modify)
			{
				piece.UpdateNode(oldNode);
				player.CheckedBy = checkedBy;
			}
			return false;
		}

		public static bool IsGuardedMove(GcPlayerMulti player, pieceMulti piece, nodeMulti tNode)
		{
			List<pieceMulti> oppPieces = GameManagerMulti.Instance.Opponent(player).Pieces;
			for (int i = 0; i < oppPieces.Count; i++)
			{
				if (oppPieces[i].IsPossibleMove(tNode))
				{
					return true;
				}
			}

			return false;
		}


		//Modifies the move if modify = true;
		public static bool IsCheckEat(GcPlayerMulti player, pieceMulti piece, nodeMulti tNode, bool modify)
		{
			nodeMulti oldNode = piece.Node;
			pieceMulti tPiece = tNode.Piece;
			tPiece.UpdateNode(null);
			piece.UpdateNode(tNode);
			pieceMulti checkedBy = player.CheckedBy;
			player.ClearCheck();
			GameManagerMulti.Instance.Opponent(player).ComputePieces();
			if (player.IsChecked)
			{
				piece.UpdateNode(oldNode);
				tPiece.UpdateNode(tNode);
				player.CheckedBy = checkedBy;
				return true;
			}

			if (!modify)
			{
				piece.UpdateNode(oldNode);
				tPiece.UpdateNode(tNode);
				player.CheckedBy = checkedBy;
			}
			return false;
		}

		public static bool HasNoMove()
		{
			GcPlayerMulti player = GameManagerMulti.Instance.CurrentPlayer;
			List<pieceMulti> pieces = player.Pieces;

			for (int i = 0; i < pieces.Count; i++)
			{
				List<nodeMulti> possibleMoves = pieces[i].PossibleMoves;
				for (int j = 0; j < possibleMoves.Count; j++)
				{
					nodeMulti tNode = possibleMoves[j];
					if (!RulesMulti.IsCheckMove(player, pieces[i], tNode, false))
					{
						return false;
					}
				}

				List<nodeMulti> possibleEats = pieces[i].PossibleEats;
				for (int j = 0; j < possibleEats.Count; j++)
				{
					nodeMulti tNode = possibleEats[j];
					if (!RulesMulti.IsCheckEat(player, pieces[i], tNode, false))
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
