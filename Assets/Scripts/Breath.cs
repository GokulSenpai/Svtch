using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using EZCameraShake;

public class Breath : MonoBehaviour
{    
    [Space]
    public Volume postVolume;
    
    [Space] [ShowOnly]
    [SerializeField] private bool heldBreath;
    
    [Space]
    [ShowOnly] [SerializeField] private float breathTimer;
    
    [Space][Header("Breath Settings")]
    [SerializeField] private float breathHoldTime = 15f;
    [SerializeField] private float breathWalkSpeed = 1f;
    [SerializeField] private float breathStrafeSpeed = 1f;

    [Space] [Header("Camera Shake Settings")]
    [SerializeField] private float shakeIncrement = 0.035f;
    [SerializeField] private float roughnessIncrement = 0.035f;

    [Space] [Header("Breath Audio")] 
    [SerializeField] private AudioSource inhale;
    [SerializeField] private AudioSource heartBeat;
    [SerializeField] private AudioSource exhale;

    [Space] [Header("Post Effect Value Changes")]
    [SerializeField] private float vignetteIncrement = 0.1f;
    [SerializeField] private float contrastDecrement = 0.25f;
    [SerializeField] private float effectRecoilTime = 5f;
    
    private FPSCharacter _player;

    private float _shakeMagnitude;
    private float _shakeRoughness;
    
    private Vignette _vignette;
    private ColorAdjustments _colorAdjustments;

    private float _playerSpeedBackup;
    private float _playerStrafeBackup;

    private bool _timeCompleted;

    private AudioClip _exhaleAudio;

    private bool _hasPlayedOnce;

    private void Awake()
    {
        _player = gameObject.GetComponent<FPSCharacter>();
        
        postVolume.profile.TryGet(out _vignette);
        postVolume.profile.TryGet(out _colorAdjustments);
    }

    private void Start()
    {
        _exhaleAudio = exhale.clip;

        _playerSpeedBackup = _player.walkSpeed;
        _playerStrafeBackup = _player.strafeSpeed;
    }

    private void Update()
    {
        heldBreath = Input.GetKey(KeyCode.LeftAlt);

        if (heldBreath)
        {
            if (breathTimer < breathHoldTime)
            {
                breathTimer += Time.deltaTime;
                IncrementEffectShakeSpeed();

                _timeCompleted = false;
            }
            else
            {
                heldBreath = false;
                _timeCompleted = true;
                
                heartBeat.Stop();

                if (!_hasPlayedOnce)
                {
                    exhale.PlayOneShot(_exhaleAudio);
                    _hasPlayedOnce = true;
                }
                
                BackToNormalEffectShakeSpeed();
            }
        }
        else
        {
            BackToNormalEffectShakeSpeed();
            breathTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            breathTimer = 0f;
            exhale.Stop();
            StartCoroutine(nameof(StartHeartBeatCoroutine));
        }

        if (!Input.GetKeyUp(KeyCode.LeftAlt) || _timeCompleted) return;
        StopHeartBeatCoroutineAndExhale();
    }

    private void IncrementEffectShakeSpeed()
    {
        _vignette.smoothness.value += vignetteIncrement * Time.deltaTime;
        _colorAdjustments.postExposure.value -= contrastDecrement * Time.deltaTime;

        // Will go to around 0.5 in 15 seconds
        _shakeMagnitude += shakeIncrement * Time.deltaTime;
        _shakeRoughness += roughnessIncrement * Time.deltaTime;

        CameraShaker.Instance.ShakeOnce(_shakeMagnitude, _shakeRoughness, 0.1f, 1f);

        _player.walkSpeed = breathWalkSpeed;
        _player.strafeSpeed = breathStrafeSpeed;
    }

    private void BackToNormalEffectShakeSpeed()
    {
        _vignette.smoothness.value = Mathf.Lerp(_vignette.smoothness.value, 0f, Time.deltaTime * effectRecoilTime);
        _colorAdjustments.postExposure.value =
            Mathf.Lerp(_colorAdjustments.postExposure.value, 0f, Time.deltaTime * effectRecoilTime);

        _shakeMagnitude = Mathf.Lerp(_shakeMagnitude, 0f, Time.deltaTime * effectRecoilTime);
        _shakeRoughness = Mathf.Lerp(_shakeRoughness, 0f, Time.deltaTime * effectRecoilTime);

        _player.walkSpeed = _playerSpeedBackup;
        _player.strafeSpeed = _playerStrafeBackup;
    }

    private IEnumerator StartHeartBeatCoroutine()
    {
        inhale.Play();
        yield return new WaitForSeconds(1f);
        heartBeat.Play();
        yield return new WaitForSeconds(14f);
        heartBeat.Stop();
    }
    
    private void StopHeartBeatCoroutineAndExhale()
    {
        StopCoroutine(nameof(StartHeartBeatCoroutine));
        heartBeat.Stop();
        exhale.Play();
    }
}
