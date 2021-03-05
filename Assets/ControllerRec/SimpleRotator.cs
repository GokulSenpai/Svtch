using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public Vector2 rotationRange = new Vector2(70, 70);
    public float rotationSpeed = 10f;

    public float dampingTime = 0.2f;

    // Final rotation for camera to ease in
    private Vector3 targetAngles;
    // Current rotation of the camera
    private Vector3 followAngles;
    private Vector3 followVelocity;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        if (targetAngles.y > 180)
        {
            targetAngles.y -= 360;
            followAngles.y -= 360;
        }
        else if (targetAngles.y < -180)
        {
            targetAngles.y += 360;
            followAngles.y += 360;
        }

        if (targetAngles.x > 180)
        {
            targetAngles.x -= 360;
            followAngles.x -= 360;
        }
        else if (targetAngles.x < -180)
        {
            targetAngles.x += 360;
            followAngles.x += 360;
        }
            
        targetAngles.y += horizontalInput * rotationSpeed;
        targetAngles.x += verticalInput * rotationSpeed;

        targetAngles.y = Mathf.Clamp(targetAngles.y, rotationRange.y * -0.5f, rotationRange.y * 0.5f);
        targetAngles.x = Mathf.Clamp(targetAngles.x, rotationRange.x * -0.5f, rotationRange.x * 0.5f);

        followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, dampingTime);

        transform.localRotation = originalRotation * Quaternion.Euler(-followAngles.x, followAngles.y, 0);
    }
}
