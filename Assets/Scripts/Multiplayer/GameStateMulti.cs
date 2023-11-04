using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.impactional.chess
{
    public enum GameStateTypeMulti
    {
        WAITING,
        HOLDING,
        PLACING,
        GAME_OVER
    }

    public enum GameOverTypeMulti
    {
        CHECKMATE,
        STALEMATE,
        SURRENDER,
        OUT_OF_TIME,
    }

    public class GameStateMulti : MonoBehaviour
    {
		private GameStateTypeMulti state;
		private GameOverTypeMulti gameOverType;

		public GameStateMulti()
		{
			state = GameStateTypeMulti.WAITING;
		}

		public GameStateTypeMulti State
		{
			get { return state; }
			set
			{
				state = value;
			}
		}

		public bool IsWaiting
		{
			get { return state == GameStateTypeMulti.WAITING; }
		}

		public bool IsPlacing
		{
			get { return state == GameStateTypeMulti.PLACING; }
		}

		public bool IsHolding
		{
			get { return state == GameStateTypeMulti.HOLDING; }
		}

		public void Grab()
		{
			state = GameStateTypeMulti.HOLDING;
		}
		
		public void Place()
		{
			state = GameStateTypeMulti.PLACING;
		}

		public void Release()
		{
			state = GameStateTypeMulti.WAITING;
			GameManagerMulti.Instance.SwitchPlayer();
		}

		public void Cancel()
		{
			state = GameStateTypeMulti.WAITING;
		}

		public void Checkmate()
		{
			state = GameStateTypeMulti.GAME_OVER;
			gameOverType = GameOverTypeMulti.CHECKMATE;
		}

		public void OutOfTime()
		{
			state = GameStateTypeMulti.GAME_OVER;
			gameOverType = GameOverTypeMulti.OUT_OF_TIME;
		}

		public void Stalemate()
		{
			state = GameStateTypeMulti.GAME_OVER;
			gameOverType = GameOverTypeMulti.STALEMATE;
		}

		public bool IsGameOver
		{
			get
			{
				if (state == GameStateTypeMulti.GAME_OVER) return true;
				return false;
			}
		}
	}
}
