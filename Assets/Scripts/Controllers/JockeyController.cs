using System.Collections;
using System.Collections.Generic;
using TF0b.Input;
using UnityEngine;

namespace TF0b.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
	public class JockeyController : MonoBehaviour
	{
        [SerializeField] private float acceleration = 5.0f;
        [SerializeField] private float maxLook = 90.0f;
        [SerializeField] private float runSpeed = 3.0f;
        [SerializeField] private CameraController fpsCamera;
        
        private float cameraXRot = 0.0f;
        private InputManager inputManager;
        private new Rigidbody rigidbody;
        private Vector2 movement = Vector2.zero;

        void Awake()
        {
            inputManager = FindObjectOfType<InputManager>();
            rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            fpsCamera.MakeCurrent();
        }

        void FixedUpdate()
        {
            DoCamera();
            DoMove();
        }

        private void DoCamera()
        {
            Transform t = fpsCamera.transform;
			Vector2 mouseDelta = inputManager.CurrentLook * inputManager.LookSensitivity * Time.deltaTime;

            cameraXRot = Mathf.Clamp(cameraXRot - mouseDelta.y, -maxLook, maxLook);
            fpsCamera.transform.localEulerAngles = new Vector3(cameraXRot, 0.0f, 0.0f);

            transform.Rotate(0, mouseDelta.x, 0);
        }

        private void DoMove()
        {
			movement = Vector2.MoveTowards(movement, inputManager.CurrentMove, acceleration * Time.deltaTime);
			Vector3 velocity = new Vector3(movement.x * runSpeed, rigidbody.velocity.y, movement.y * runSpeed);
			rigidbody.velocity = transform.rotation * velocity;
        }
	}
}
