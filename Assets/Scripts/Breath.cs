using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using EZCameraShake;

public class Breath : MonoBehaviour
{    
    [Space]
    public Volume postVolume;
    private FPSCharacter player;
    
    [Space][Header("Breath Settings")]
    public bool heldBreath = false;
    public float maximumBreathHoldTime = 15f;
    public float breathTimer = 0f;
    public float breathWalkSpeed = 1f;
    public float breathStrafeSpeed = 1f;

    [Space] [Header("Camera Shake Settings")]
    public float shakeIncrement = 0.035f;
    public float roughnessIncrement = 0.035f;


    [Space] [Header("Breath Audio")] 
    public AudioSource inhale;
    public AudioSource heartBeat;
    public AudioSource exhale;

    [Space] [Header("Post Effect Value Changes")]
    public float vignetteIncrement = 0.1f;
    public float contrastDecrement = 0.25f;
    public float effectRecoilTime = 5f;

    private float shakeMagnitude = 0f;
    private float shakeRoughness = 0f;
    
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;

    private float playerSpeedBackup;
    private float playerStrafeBackup;

    private bool timeCompleted = false;

    private AudioClip exhaleAudio;

    private bool hasPlayedOnce = false;

    private void Start()
    {
        player = gameObject.GetComponent<FPSCharacter>();

        exhaleAudio = exhale.clip;

        playerSpeedBackup = player.walkSpeed;
        playerStrafeBackup = player.strafeSpeed;
        
        postVolume.profile.TryGet(out vignette);
        postVolume.profile.TryGet(out colorAdjustments);
    }
    
    void Update()
    {
        heldBreath = Input.GetKey(KeyCode.LeftAlt);

        if (heldBreath)
        {
            if (breathTimer < maximumBreathHoldTime)
            {
                breathTimer += Time.deltaTime;
                vignette.smoothness.value += vignetteIncrement * Time.deltaTime;
                colorAdjustments.postExposure.value -= contrastDecrement * Time.deltaTime;
                
                // Will go to around 0.5 in 15 seconds
                shakeMagnitude += shakeIncrement * Time.deltaTime;
                shakeRoughness += roughnessIncrement * Time.deltaTime;
                
                CameraShaker.Instance.ShakeOnce(shakeMagnitude, shakeRoughness, 0.1f, 1f);
                
                player.walkSpeed = breathWalkSpeed;
                player.strafeSpeed = breathStrafeSpeed;

                timeCompleted = false;
            }
            else
            {
                heldBreath = false;
                timeCompleted = true;
                
                heartBeat.Stop();

                if (!hasPlayedOnce)
                {
                    exhale.PlayOneShot(exhaleAudio);
                    hasPlayedOnce = true;
                }
                
                vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0f, Time.deltaTime * effectRecoilTime);
                colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, 0f, Time.deltaTime * effectRecoilTime);

                shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, Time.deltaTime * effectRecoilTime);
                shakeRoughness = Mathf.Lerp(shakeRoughness, 0f, Time.deltaTime * effectRecoilTime);
                
                player.walkSpeed = playerSpeedBackup;
                player.strafeSpeed = playerStrafeBackup;
                
                // Death state
            }
        }
        else
        {
                vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0f, Time.deltaTime * effectRecoilTime);
                colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, 0f, Time.deltaTime * effectRecoilTime);
                
                shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, Time.deltaTime * effectRecoilTime);
                shakeRoughness = Mathf.Lerp(shakeRoughness, 0f, Time.deltaTime * effectRecoilTime);
                
                player.walkSpeed = playerSpeedBackup;
                player.strafeSpeed = playerStrafeBackup;
                
                breathTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            breathTimer = 0f;
            exhale.Stop();
            StartCoroutine(nameof(StartHeartBeat));
        }
        
        if (Input.GetKeyUp(KeyCode.LeftAlt) && !timeCompleted)
        {
            StopCoroutine(nameof(StartHeartBeat));
            heartBeat.Stop();
            exhale.Play();
        }
    }

    private IEnumerator StartHeartBeat()
    {
        inhale.Play();
        yield return new WaitForSeconds(1f);
        heartBeat.Play();
        yield return new WaitForSeconds(14f);
        heartBeat.Stop();
    }
}
