using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FPSCharacter : MonoBehaviour
{
    public float strafeSpeed = 2f;
    public float walkSpeed = 2f;
    
    [SerializeField] private AdvancedSettings advanced = new AdvancedSettings();

    [System.Serializable]
    public class AdvancedSettings
    {
        public float gravityMultiplier = 1.0f;
        public PhysicMaterial zeroFrictionMaterial;
        public PhysicMaterial highFrictionMaterial;
    }

    private CapsuleCollider capsule;
    private Rigidbody _rigidbody;
    private const float JumpRayLength = 0.7f;

    public bool Grounded
    {
        get; 
        private set;
    }

    private Vector2 input;

    private void Awake()
    {
        capsule = GetComponent<Collider>() as CapsuleCollider;
        _rigidbody = GetComponent<Rigidbody>();
        Grounded = true;
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Grounded || _rigidbody.velocity.y < 0.1f)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, capsule.height * JumpRayLength);
            float nearest = float.PositiveInfinity;
            Grounded = false;

            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].collider.isTrigger && hits[i].distance < nearest)
                {
                    Grounded = true;
                    nearest = hits[i].distance;
                }
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        input = new Vector2(horizontalInput, verticalInput);

        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        Vector3 desiredMove = transform.forward * input.y * walkSpeed + transform.right * input.x * strafeSpeed;

        _rigidbody.velocity = desiredMove + Vector3.up * _rigidbody.velocity.y;

        if (desiredMove.magnitude > 0 || !Grounded)
        {
            capsule.material = advanced.zeroFrictionMaterial;
        }
        else
        {
            capsule.material = advanced.highFrictionMaterial;
        }
        
        _rigidbody.AddForce(Physics.gravity * (advanced.gravityMultiplier - 1));

    }
}
