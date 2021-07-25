using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TF0b.Input
{
	public class InputManager : MonoBehaviour
	{
		public bool IsCursorVisible { get => Cursor.visible; set => Cursor.visible = value; }
		public CursorLockMode CursorLock { get => Cursor.lockState; set => Cursor.lockState = value; }
		public float LookSensitivity { get => lookSensitivity * sensitivityFudge; set => SetLookSensitivity(value); }
		public Vector2 CurrentMove { get; private set; }
		public Vector2 CurrentLook { get; private set; }

		[SerializeField] private float sensitivityFudge = 50.0f;
		private float lookSensitivity = 1.0f;

		void OnMove(InputValue value) => CurrentMove = value.Get<Vector2>();
		void OnLook(InputValue value) => CurrentLook = value.Get<Vector2>();

		void Start()
		{
			IsCursorVisible = false;
			CursorLock = CursorLockMode.Locked;
		}

		void Update()
		{
			// This is for testing and is to be replaced with a menu system later.
			if(Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				IsCursorVisible = !IsCursorVisible;
				if(IsCursorVisible) CursorLock = CursorLockMode.None;
				else CursorLock = CursorLockMode.Locked;
			}
		}

		public void SetLookSensitivity(float value)
		{
			if(value <= 0.0f) value = 1.0f;
			
			lookSensitivity = value;
		}
	}
}
