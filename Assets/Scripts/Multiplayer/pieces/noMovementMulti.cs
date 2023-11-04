using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class noMovementMulti : movementMulti, IPieceMovement
    {

		public noMovementMulti(GcPlayerMulti player, pieceMulti piece) : base(player, piece)
		{
			BoundComputations += ComputeBound;
		}



		public override void ComputeBound()
		{
			//do nothing
		}
	}
}
