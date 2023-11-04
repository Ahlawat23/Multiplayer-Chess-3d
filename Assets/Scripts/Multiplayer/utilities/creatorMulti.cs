using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class creatorMulti : MonoBehaviour
    {
		public static IPieceMovement CreatePieceMovement(MovementTypeMulti movementType, GcPlayerMulti player, pieceMulti piece)
		{
			switch (movementType)
			{
                
				case MovementTypeMulti.KING:
                    return new kingMovementMulti(player, piece);
                case MovementTypeMulti.PAWN:
                    return new pawnMovementMulti(player, piece);
                case MovementTypeMulti.ROOK:
                    return new rookMovementMulti(player, piece);
                case MovementTypeMulti.BISHOP:
                    return new bishopMovementMulti(player, piece);
                case MovementTypeMulti.QUEEN:
                    return new queenMovementMulti(player, piece);
                case MovementTypeMulti.KNIGHT:
                    return new knightMovementMulti(player, piece);

                case MovementTypeMulti.CIRCLE:
                    return new circleMovementMulti(player, piece);
                case MovementTypeMulti.CROSS:
                    return new crossMovementMulti(player, piece);
                case MovementTypeMulti.NONE:
				default:
                    return new noMovementMulti(player, piece);
            }
		}
	}
}
