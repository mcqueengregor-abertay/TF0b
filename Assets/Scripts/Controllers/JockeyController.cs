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
        [SerializeField] private float sprintFovIncrease = 5.0f;
        [SerializeField] private float sprintSpeedMultiplier = 1.5f;
        [SerializeField] private CameraController fpsCamera;
        
        private Camera mainCamera;
        private float baseFov;
        private float cameraXRot = 0.0f;
        private float targetFov;
        private InputManager inputManager;
        private new Rigidbody rigidbody;
        private Vector2 movement = Vector2.zero;

        void Awake()
        {
            mainCamera = Camera.main;
            baseFov = mainCamera.fieldOfView;
            inputManager = FindObjectOfType<InputManager>();
            rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            fpsCamera.MakeCurrent();
            targetFov = baseFov;
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

            if(inputManager.IsSprinting && movement.sqrMagnitude > 0.0f) targetFov = baseFov + sprintFovIncrease;
            else targetFov = baseFov;

            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFov, Time.deltaTime * sprintFovIncrease);
        }

        private void DoMove()
        {
            float sprintMul = inputManager.IsSprinting ? sprintSpeedMultiplier : 1.0f;
			movement = Vector2.MoveTowards(movement, inputManager.CurrentMove * sprintMul, acceleration * Time.deltaTime);
			Vector3 velocity = new Vector3(movement.x * runSpeed, rigidbody.velocity.y, movement.y * runSpeed);
			rigidbody.velocity = transform.rotation * velocity;
        }
	}
}
