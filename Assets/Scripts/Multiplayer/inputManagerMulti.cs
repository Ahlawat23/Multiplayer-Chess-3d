using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace com.impactional.chess
{

    public enum InputActionTypeMulti
    {
        GRAB_PIECE = 0,
        PLACE_PIECE = 1,
        CANCEL_PIECE = 2,
        ZOOM_IN = 3,
        ZOOM_OUT = 4,
        ROTATE = 5,
        STOP_ROTATE = 6,
    }

    public class inputManagerMulti : MonoBehaviourPun
    {
		public bool destroyOnLoad;
		protected static bool _destroyOnLoad;

		private static inputManagerMulti instance;

		public static inputManagerMulti Instance
		{
			get
			{
				inputManagerMulti foundObject = FindObjectOfType<inputManagerMulti>();

				if (instance == null)
				{
					instance = foundObject;
				}
				else if (instance != foundObject)
				{
					Destroy(foundObject);
				}

				if (!_destroyOnLoad) DontDestroyOnLoad(foundObject);
				return instance;
			}
		}









		public delegate void InputEventHandlerMulti(InputActionTypeMulti actionType);
		public static event InputEventHandlerMulti InputEvent;

		private bool clicked;
		private nodeMulti currentNode;
		private GcPlayerMulti currentPlayer;

		public Vector2 mouseAxis;

		public Vector2 MouseAxis
		{
			get { return mouseAxis; }
		}

		void Awake()
		{
			_destroyOnLoad = destroyOnLoad;
			mouseAxis = new Vector2(0, 0);
		}

		void OnDisable()
		{
			InputEvent = null;
		}

		void Update()
		{
			mouseAxis.x = Input.GetAxis("Mouse X");
			mouseAxis.y = Input.GetAxis("Mouse Y");

			//Debug.Log("The InputEvent is equal to null : " + InputEvent == null);
			if (InputEvent == null) return;



            Debug.Log("The gameManagerMuti instance is not ready : " + !GameManagerMulti.Instance.IsReady);
            if (!GameManagerMulti.Instance.IsReady) return;

			HighlightTile();

			if (Input.GetMouseButtonUp(0))
			{

				if (GameManagerMulti.Instance.GameState.IsWaiting)
				{
					UnHighlightTile();
					InputEvent(InputActionTypeMulti.GRAB_PIECE);
					Debug.Log("game state is waiting");
				}
				else if (GameManagerMulti.Instance.GameState.IsHolding)
				{
					InputEvent(InputActionTypeMulti.PLACE_PIECE);
					Debug.Log("gamestae is holding");
				}
			}

			if (Input.GetMouseButtonUp(1))
			{
				if (GameManagerMulti.Instance.GameState.IsHolding)
				{
					InputEvent(InputActionTypeMulti.CANCEL_PIECE);
				}
			}

			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				InputEvent(InputActionTypeMulti.ZOOM_IN);
			}

			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				InputEvent(InputActionTypeMulti.ZOOM_OUT);
			}

			if (Input.GetMouseButtonDown(2))
			{
				InputEvent(InputActionTypeMulti.ROTATE);
			}
			else if (Input.GetMouseButtonUp(2))
			{
				InputEvent(InputActionTypeMulti.STOP_ROTATE);
			}
		}


		[PunRPC]
		public void onmouseClickInput()
        {
			
		}
		

		public void HighlightTile()
		{
			if (GameManagerMulti.Instance.GameState.IsWaiting)
			{
				UnHighlightTile();
				currentNode = finderMulti.RayHitFromScreen<nodeMulti>(Input.mousePosition);
				if (currentNode != null)
				{
					pieceMulti piece = currentNode.Piece;
					if (piece != null)
					{
						if (GameManagerMulti.Instance.CurrentPlayer.Has(piece))
						{
							currentNode.HighlightMove();
						}
						else
						{
							currentNode.HighlightEat();
						}
					}
				}
			}
		}

		public void UnHighlightTile()
		{
			if (currentNode != null)
			{
				currentNode.UnhighlightEat();
				currentNode.UnhighlightMove();
			}
		}
	}
}
