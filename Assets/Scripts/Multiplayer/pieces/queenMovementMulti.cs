using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class queenMovementMulti : movementMulti, IPieceMovement
    {
		private IPieceMovement rook;
		private IPieceMovement bishop;

		public queenMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player, piece)
		{
			rook = new rookMovementMulti(player, piece);
			bishop = new bishopMovementMulti(player, piece);
			BoundComputations += rook.ComputeBound;
			BoundComputations += bishop.ComputeBound;
		}

		public override void ComputeBound()
		{
			//do nothing
		}
	}
}
