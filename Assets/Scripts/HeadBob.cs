using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
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

    private Vector3 _originalLocalPosition;
    private float _nextStepTime = 0.5f;
    private float _headBobCycle;
    private float _headBobFade;

    private float _springPosition;
    private float _springVelocity;
    
    private const float SpringElastic = 1.1f;
    private const float SpringDampen = 0.8f;
    private const float SpringVelocityThreshold = 0.05f;
    private const float SpringPositionThreshold = 0.05f;

    private Vector3 _previousPosition;
    private Vector3 _previousVelocity = Vector3.zero;

    private AudioSource _audioSource;
    private Rigidbody _rigidbody;

    private Vector3 _position;
    private Vector3 _velocity;
    private Vector3 _velocityChange;
    
    private float _flatVelocity;
    private float _strideLengthen;
    private float _bobFactor;
    private float _bobSwayFactor;
    private float _speedHeightFactor;

    private float _xPos;
    private float _yPos;

    private float _xTilt;
    private float _zTilt;

    private int _randomNumber;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _originalLocalPosition = head.localPosition;
        _previousPosition = _rigidbody.position;
    }

    private void FixedUpdate()
    {
        _position = _rigidbody.position;
        
        _velocity = (_position - _previousPosition) / Time.deltaTime;
        _velocityChange = _velocity - _previousVelocity;
        
        _previousPosition = _position;
        _previousVelocity = _velocity;

        _springVelocity -= _velocityChange.y;
        _springVelocity -= _springPosition * SpringElastic;
        _springVelocity *= SpringDampen;
        
        _springPosition += _springVelocity * Time.deltaTime;
        _springPosition = Mathf.Clamp(_springPosition, -0.3f, 0.3f);

        if (Mathf.Abs(_springVelocity) < SpringVelocityThreshold && Mathf.Abs(_springPosition) < SpringPositionThreshold)
        {
            _springVelocity = 0;
            _springPosition = 0;
        }

        _flatVelocity = new Vector3(_velocity.x, 0f, _velocity.z).magnitude;

        _strideLengthen = 1 + (_flatVelocity * bobStrideSpeedLengthen);

        _headBobCycle += _flatVelocity / _strideLengthen * (Time.deltaTime / headBobFrequency);

        _bobFactor = Mathf.Sin(_headBobCycle * Mathf.PI * 2);
        _bobSwayFactor = Mathf.Sin(Mathf.PI * (2 * _headBobCycle + 0.5f));

        _bobFactor = 1 - (_bobFactor * 0.5f + 1);
        _bobFactor *= _bobFactor;
        
        _headBobFade = Mathf.Lerp(_headBobFade, new Vector3(_velocity.x, 0f, _velocity.z).magnitude < 0.1f ? 0.0f : 1.0f, Time.deltaTime);

        _speedHeightFactor = 1 + (_flatVelocity * headBobSpeedMultiplier);

        _xPos = -headBobSideMovement * _bobSwayFactor;
        _yPos = _springPosition + _bobFactor * headBobHeight * _headBobFade * _speedHeightFactor;

        _xTilt = -_springPosition;
        _zTilt = _bobSwayFactor * headBobSwayAngle * _headBobFade;

        head.localPosition = _originalLocalPosition + new Vector3(_xPos, _yPos, 0);
        head.localRotation = Quaternion.Euler(_xTilt, 0.0f, _zTilt);

        if (!(_headBobCycle > _nextStepTime)) return;
        PlayFootStepsBasedOnHeadBob();
    }

    private void PlayFootStepsBasedOnHeadBob()
    {
        _nextStepTime = _headBobCycle + 0.5f;

        _randomNumber = Random.Range(1, footStepSounds.Length);
        _audioSource.clip = footStepSounds[_randomNumber];
        _audioSource.Play();

        footStepSounds[_randomNumber] = footStepSounds[0];
        footStepSounds[0] = _audioSource.clip;
    }
}
