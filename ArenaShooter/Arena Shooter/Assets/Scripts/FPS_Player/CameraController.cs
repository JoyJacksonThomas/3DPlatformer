using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationX = 0f;
    public float rotationY = 0f;
    public float rotationZ = 0f;
    Quaternion originalRotation;
    Camera mCamera;

    void Start()
    {
        originalRotation = transform.localRotation;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        mCamera = GetComponent<Camera>();

    }
    void Update()
    {
        

        rotationX = Mathf.Clamp(rotationX, -90, 90);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
    }

    public void AddRotation(float x, float y, float z, float sensitivity)
    {
        rotationX += x * sensitivity;
        rotationY += y * sensitivity;
        rotationZ += z * sensitivity;
    }

    

}
