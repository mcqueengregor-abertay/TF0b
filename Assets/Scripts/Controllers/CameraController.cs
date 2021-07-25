using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Current { get; private set; }

    private Transform mainCamera;

    void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if(Current != this) return;

        mainCamera.position = transform.position;
        mainCamera.rotation = transform.rotation;
    }

    public void MakeCurrent() => Current = this;
}
