using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace TF0b.Input
{
	public class InputManager : MonoBehaviour
	{
		public Vector2 CurrentMove { get; private set; }
		public Vector2 CurrentLook { get; private set; }

		void OnMove(InputValue value) => CurrentMove = value.Get<Vector2>();
		void OnLook(InputValue value) => CurrentLook = value.Get<Vector2>();
	}
}
