using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public Vector2 rotationRange = new Vector2(70, 70);
    public float rotationSpeed = 10f;

    public float dampingTime = 0.2f;

    // Final rotation for camera to ease in to
    private Vector3 _targetAngles;
    
    // Current rotation of the camera
    private Vector3 _followAngles;
    
    private Vector3 _followVelocity;

    private Quaternion _originalRotation;

    private float _horizontalInput;
    private float _verticalInput;

    private void Start()
    {
        _originalRotation = transform.localRotation;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Mouse X");
        _verticalInput = Input.GetAxis("Mouse Y");

        LimitTargetAndFollowAngles();

        _targetAngles.y += _horizontalInput * rotationSpeed;
        _targetAngles.x += _verticalInput * rotationSpeed;

        _targetAngles.y = Mathf.Clamp(_targetAngles.y, rotationRange.y * -0.5f, rotationRange.y * 0.5f);
        _targetAngles.x = Mathf.Clamp(_targetAngles.x, rotationRange.x * -0.5f, rotationRange.x * 0.5f);

        _followAngles = Vector3.SmoothDamp(_followAngles, _targetAngles, ref _followVelocity, dampingTime);

        transform.localRotation = _originalRotation * Quaternion.Euler(-_followAngles.x, _followAngles.y, 0);
    }

    private void LimitTargetAndFollowAngles()
    {
        if (_targetAngles.y > 180)
        {
            _targetAngles.y -= 360;
            _followAngles.y -= 360;
        }
        else if (_targetAngles.y < -180)
        {
            _targetAngles.y += 360;
            _followAngles.y += 360;
        }

        if (_targetAngles.x > 180)
        {
            _targetAngles.x -= 360;
            _followAngles.x -= 360;
        }
        else if (_targetAngles.x < -180)
        {
            _targetAngles.x += 360;
            _followAngles.x += 360;
        }
    }
}
