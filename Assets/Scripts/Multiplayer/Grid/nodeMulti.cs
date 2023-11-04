using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
	public class nodeMulti : Scalable, IHeapItem<nodeMulti>, IClickable
	{
		public int row;
		public int col;

		public char rowChess;
		public char colChess;

		//public bool walkable = true;

		public int gCost;
		public int hCost;

		private int heapIndex;

		private pieceMulti piece;

		protected override void Start()
		{
			base.Start();
		}

		public string ChessCoords
		{
			get { return "" + colChess + rowChess; }
		}

		public pieceMulti Piece
		{
			get { return piece; }
			set
			{
				piece = value;
			}
		}

		public void HighlightMove()
		{
			if (renderer.sharedMaterial != origMaterial) return;
			SetMaterial(GameManagerMulti.Instance.HighlightMoveMaterial);
		}

		public void HighlightEat()
		{
			if (renderer.sharedMaterial != origMaterial) return;
			SetMaterial(GameManagerMulti.Instance.HighlightEatMaterial);
		}

		public void HighlightCheck()
		{
			if (renderer.sharedMaterial != origMaterial) return;
			SetMaterial(GameManagerMulti.Instance.HighlightCheckMaterial);
		}

		public void UnhighlightMove()
		{
			Unhiglight(GameManagerMulti.Instance.HighlightMoveMaterial);
		}

		public void UnhighlightEat()
		{
			Unhiglight(GameManagerMulti.Instance.HighlightEatMaterial);
		}

		public void UnhighlightCheck()
		{
			Unhiglight(GameManagerMulti.Instance.HighlightCheckMaterial);
		}

		private void Unhiglight(Material material)
		{
			if (renderer.sharedMaterial == material)
			{
				SetMaterialOriginal();
			}
		}

		public bool EmptySpace
		{
			get
			{
				return piece == null;
			}
		}

		public void Clear()
		{
			piece = null;
		}

		public int fCost
		{
			get
			{
				return gCost + hCost;
			}
		}

		public int HeapIndex
		{
			get
			{
				return heapIndex;
			}
			set
			{
				heapIndex = value;
			}
		}

		public bool Inform<T>(T arg)
		{
			//TODO
			return true;
		}

		public int CompareTo(nodeMulti nodeToCompare)
		{
			int compare = fCost.CompareTo(nodeToCompare.fCost);
			if (compare == 0)
			{
				compare = hCost.CompareTo(nodeToCompare.hCost);
			}

			return compare;
		}

		public override string ToString()
		{
			return "" + row + "x" + col;
		}
	}
}
