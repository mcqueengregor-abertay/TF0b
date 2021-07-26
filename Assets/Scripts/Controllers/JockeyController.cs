using System.Collections;
using System.Collections.Generic;
using TF0b.Input;
using UnityEngine;

namespace TF0b.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
	public class JockeyController : MonoBehaviour
	{
        [SerializeField] private float acceleration = 10.0f;
        [SerializeField] private float crouchRate = 2.0f;
		[SerializeField] private float crouchSpeedMultiplier = 0.5f;
        [SerializeField] private float maxLook = 90.0f;
        [SerializeField] private float runSpeed = 3.0f;
        [SerializeField] private float slideSpeedMultiplier = 3.0f;
        [SerializeField] private float sprintFovIncrease = 5.0f;
        [SerializeField] private float sprintSpeedMultiplier = 1.5f;
        [SerializeField] private CameraController fpsCamera;
        [SerializeField] private new CapsuleCollider collider;
        
        private Camera mainCamera;
        private float baseFov;
        private float cameraXRot = 0.0f;
        private float targetFov;
        private InputManager inputManager;
        private new Rigidbody rigidbody;
        private Vector3 movement = Vector3.zero;
        private Vector3 slideVector = Vector3.zero;

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
            inputManager.OnCrouchDown.AddListener(OnCrouch);
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
            float speedMul = 1.0f;
            if(inputManager.IsCrouching) speedMul = crouchSpeedMultiplier;
            else if(inputManager.IsSprinting) speedMul = sprintSpeedMultiplier;

            Vector3 currentMove = inputManager.CurrentMove * runSpeed * speedMul;
            movement = Vector3.MoveTowards(movement, new Vector3(currentMove.x, 0.0f, currentMove.y), Time.deltaTime * acceleration);
            movement.y = rigidbody.velocity.y;
            rigidbody.velocity = transform.rotation * movement + slideVector;

            slideVector = Vector3.MoveTowards(slideVector, Vector3.zero, Time.deltaTime * acceleration);

            float colliderTarget = inputManager.IsCrouching ? 1.0f : 2.0f;
            collider.height = Mathf.MoveTowards(collider.height, colliderTarget, Time.deltaTime * crouchRate);
        }

        private void OnCrouch()
        {
            if(rigidbody.velocity.sqrMagnitude > runSpeed * runSpeed)
            {
				slideVector = rigidbody.velocity * slideSpeedMultiplier;
            }
        }
	}
}
