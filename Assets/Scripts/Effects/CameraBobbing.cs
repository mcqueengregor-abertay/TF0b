using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
	[SerializeField] private float bobScale = 0.05f;
	[SerializeField] private float bobSpeed = 10.0f;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private new Rigidbody rigidbody;

    private Transform anchor;
    private Vector3 anchorBase;

    void Awake()
    {
        anchor = cameraController.transform;
        anchorBase = anchor.localPosition;
    }

    void Update()
    {
        float vMag = rigidbody.velocity.magnitude;
        float xOffset = Mathf.Sin(Time.time * bobSpeed) * vMag * bobScale / 4.0f;
        float yOffset = Mathf.Sin(Time.time * 2.0f * bobSpeed) * vMag * bobScale / 3.0f;
        
        Vector3 offset = new Vector3(xOffset, yOffset, 0.0f);
        anchor.localPosition = anchorBase + offset;
    }
}
