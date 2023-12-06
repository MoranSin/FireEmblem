using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SC_CameraControl : MonoBehaviour
{
    [SerializeField] float keyboardInputSensitivity = 1f;
    [SerializeField] float mouseInputSensitivity = 1f;
    Vector3 input;
    Vector3 pointOfOrigin;

    [SerializeField] bool continious = true;

    [SerializeField] Transform bottomLeftBorder;
    [SerializeField] Transform topRightBorder;


    private void Update()
    {
        NullInput();

        MoveCameraInput();

        MoveCamera();
    }

    private void NullInput()
    {
        input.x = 0;
        input.y = 0;
        input.z = 0;
    }

    private void MoveCamera()
    {
        Vector3 position = transform.position;
        position += (input * Time.deltaTime);
        position.x = Mathf.Clamp(position.x, bottomLeftBorder.position.x, topRightBorder.position.x);
        position.z = Mathf.Clamp(position.z, bottomLeftBorder.position.z, topRightBorder.position.z);

        transform.position = position;
    }

    private void MoveCameraInput()
    {
        AxisInput();
        MouseInput();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointOfOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseInput = Input.mousePosition;
            input.x += (mouseInput.x - pointOfOrigin.x) * mouseInputSensitivity;
            input.z += (mouseInput.y - pointOfOrigin.y) * mouseInputSensitivity;
            if (continious == false)
            {
                pointOfOrigin = mouseInput;
            }
        }
    }

    private void AxisInput()
    {
        input.x += Input.GetAxisRaw("Horizontal") * keyboardInputSensitivity;
        input.z += Input.GetAxisRaw("Vertical") * keyboardInputSensitivity;
    }
}
