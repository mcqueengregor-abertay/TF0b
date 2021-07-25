using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TF0b.Input
{
	public class InputManager : MonoBehaviour
	{
		public float LookSensitivity { get => lookSensitivity * sensitivityFudge; set => SetLookSensitivity(value); }
		public Vector2 CurrentMove { get; private set; }
		public Vector2 CurrentLook { get; private set; }

		[SerializeField] private float sensitivityFudge = 50.0f;
		private float lookSensitivity = 1.0f;

		void OnMove(InputValue value) => CurrentMove = value.Get<Vector2>();
		void OnLook(InputValue value) => CurrentLook = value.Get<Vector2>();

		public void SetLookSensitivity(float value)
		{
			if(value <= 0.0f) value = 1.0f;
			
			lookSensitivity = value;
		}
	}
}
