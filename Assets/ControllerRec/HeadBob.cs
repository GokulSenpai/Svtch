using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private Transform head;
    
    [SerializeField] private float headBobFrequency = 1.5f;
    [SerializeField] private float headBobSwayAngle = 0.5f;
    [SerializeField] private float headBobHeight = 0.3f;
    [SerializeField] private float headBobSideMovement = 0.05f;
    [SerializeField] private float headBobSpeedMultiplier = 0.3f;
    [SerializeField] private float bobStrideSpeedLengthen = 0.3f;
    
    [SerializeField] private AudioClip[] footStepSounds;

    private Vector3 originalLocalPosition;
    private float nextStepTime = 0.5f;
    private float headBobCycle = 0.0f;
    private float headBobFade = 0.0f;

    private float springPosition = 0.0f;
    private float springVelocity = 0.0f;
    private float srpingElastic = 1.1f;
    private float springDampen = 0.8f;
    private float springVelocityThreshold = 0.05f;
    private float springPositionThreshold = 0.05f;
    private Vector3 previousPosition;
    private Vector3 previousVelocity = Vector3.zero;

    private AudioSource _audioSource;
    private Rigidbody _rigidbody;

    private void Start()
    {
        originalLocalPosition = head.localPosition;

        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }

        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        previousPosition = _rigidbody.position;
    }

    private void FixedUpdate()
    {
        var position = _rigidbody.position;
        
        Vector3 velocity = (position - previousPosition) / Time.deltaTime;
        Vector3 velocityChange = velocity - previousVelocity;
        
        previousPosition = position;
        previousVelocity = velocity;

        springVelocity -= velocityChange.y;
        springVelocity -= springPosition * srpingElastic;
        springVelocity *= springDampen;
        
        springPosition += springVelocity * Time.deltaTime;
        springPosition = Mathf.Clamp(springPosition, -0.3f, 0.3f);

        if (Mathf.Abs(springVelocity) < springVelocityThreshold && Mathf.Abs(springPosition) < springPositionThreshold)
        {
            springVelocity = 0;
            springPosition = 0;
        }

        float flatVelocity = new Vector3(velocity.x, 0f, velocity.z).magnitude;

        float strideLengthen = 1 + (flatVelocity * bobStrideSpeedLengthen);

        headBobCycle += (flatVelocity / strideLengthen) * (Time.deltaTime / headBobFrequency);

        float bobFactor = Mathf.Sin(headBobCycle * Mathf.PI * 2);
        float bobSwayFactor = Mathf.Sin(Mathf.PI * (2 * headBobCycle + 0.5f));

        bobFactor = 1 - (bobFactor * 0.5f + 1);
        bobFactor *= bobFactor;

        if (new Vector3(velocity.x, 0f, velocity.z).magnitude < 0.1f)
        {
            headBobFade = Mathf.Lerp(headBobFade, 0.0f, Time.deltaTime);
        }
        else
        {
            headBobFade = Mathf.Lerp(headBobFade, 1.0f, Time.deltaTime);
        }

        float speedHeightFactor = 1 + (flatVelocity * headBobSpeedMultiplier);

        float xPos = -headBobSideMovement * bobSwayFactor;
        float yPos = springPosition + bobFactor * headBobHeight * headBobFade * speedHeightFactor;

        float xTilt = -springPosition;
        float zTilt = bobSwayFactor * headBobSwayAngle * headBobFade;

        head.localPosition = originalLocalPosition + new Vector3(xPos, yPos, 0);
        head.localRotation = Quaternion.Euler(xTilt, 0.0f, zTilt);

        if (headBobCycle > nextStepTime)
        {
            nextStepTime = headBobCycle + 0.5f;

            int n = Random.Range(1, footStepSounds.Length);
            _audioSource.clip = footStepSounds[n];
            _audioSource.Play();

            footStepSounds[n] = footStepSounds[0];
            footStepSounds[0] = _audioSource.clip;
        }
    }
}
