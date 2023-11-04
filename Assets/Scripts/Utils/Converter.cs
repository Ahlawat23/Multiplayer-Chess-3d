using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter {

	public static string ToChessCoords(int row, int col) {
		return "" + ToChessCol(col) + ToChessRow(row);
	}

	public static char ToChessRow(int row) {
		char cRow = '1';
		int iRow = cRow + row;
		return (char) iRow;
	}

	public static char ToChessCol(int col) {
		char cCol = 'a';
		int iCol = cCol + col;
		return (char) iCol;
	}

	public static Ray ScreenPointToRay(Vector3 pointPosition) {

		if(GameManager.Instance == null)
		{
			Debug.Log("in converter script the GameManager  is null therfore the GameManagerMUlti instance has been used");
			return com.impactional.chess.GameManagerMulti.Instance.MainCamera.ScreenPointToRay(pointPosition);
        }
	    else 
		{
			Debug.Log("either the game is offline or your converter script idea was dumb");
			return GameManager.Instance.MainCamera.ScreenPointToRay(pointPosition);
		}
	}
}
