using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.impactional.chess
{
    public class RotateCameraMulti : MonoBehaviour
    {
		[SerializeField]
		private Transform target;
		[SerializeField]
		private float horizAngleMove;
		[SerializeField]
		private float vertAngleMove;

		private float lookAngleY;
		public float turnSmoothing;

		public float xAngleMax = 75f;
		public float xAngleMin = 45f;
		public float lookAngleX;

		public Transform pivotTransform;
		private Vector3 pivotEulers;

		private inputManagerMulti inputManager;
		private Quaternion newRotY;
		private Quaternion newRotX;

		private bool rotate = false;

		void Start()
		{
			pivotEulers = pivotTransform.rotation.eulerAngles;
			EnableInput();
			inputManager = inputManagerMulti.Instance;
		}

		public void EnableInput()
		{
			inputManagerMulti.InputEvent += OnInputEvent;
		}

		public void DisableInput()
		{
			inputManagerMulti.InputEvent -= OnInputEvent;
		}

		void OnDisable()
		{
			DisableInput();
		}

		protected void Update()
		{
			if (!rotate) return;
			lookAngleY += inputManager.MouseAxis.x * horizAngleMove;
			newRotY = Quaternion.Euler(0f, lookAngleY, 0f);

			lookAngleX += inputManager.MouseAxis.y * vertAngleMove;
			lookAngleX = Mathf.Clamp(lookAngleX, -xAngleMin, xAngleMax);
			newRotX = Quaternion.Euler(lookAngleX, pivotEulers.y, pivotEulers.z);

			if (turnSmoothing > 0)
			{
				pivotTransform.localRotation = Quaternion.Slerp(pivotTransform.localRotation, newRotX, turnSmoothing * Time.deltaTime);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotY, turnSmoothing * Time.deltaTime);
			}
			else
			{
				transform.localRotation = newRotY;
				pivotTransform.localRotation = newRotX;
			}
		}

		public void OnInputEvent(InputActionTypeMulti action)
		{
			switch (action)
			{
				case InputActionTypeMulti.ROTATE:
					rotate = true;
					break;

				case InputActionTypeMulti.STOP_ROTATE:
					rotate = false;

					break;
			}
		}
	}
}
