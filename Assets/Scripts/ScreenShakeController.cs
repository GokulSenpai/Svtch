using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShakeController : MonoBehaviour
{
    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;

    [Range(0, 35)]
    public float rotationMultiplier = 7.5f;

    [Range(0, 0.05f)]
    public float shakeLength = 0.05f;
    
    [Range(0, 0.1f)]
    public float shakePow = 0.1f;

    private Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        StartShake(shakeLength, shakePow);
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-0.5f, 0.5f) * shakePower;
            float yAmount = Random.Range(-0.5f, 0.5f) * shakePower;

            transform.localPosition += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        }
        else
        {
            float moveX = Mathf.MoveTowards(transform.localPosition.x, startPos.x, shakeFadeTime * 2 * Time.deltaTime);
            float moveY = Mathf.MoveTowards(transform.localPosition.y, startPos.y, shakeFadeTime * 2 * Time.deltaTime);

            transform.localPosition = new Vector3(moveX, moveY, transform.localPosition.z);
        }
        
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;    
        shakePower = power;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiplier;
    }
}
