using System;
using UnityEngine;

public class FPSCharacter : MonoBehaviour
{
    public float strafeSpeed = 2f;
    public float walkSpeed = 2f;
    
    [SerializeField] private AdvancedSettings advanced = new AdvancedSettings();
    
    private float _horizontalInput;
    private float _verticalInput;
    
    private Transform _transform;
    private CapsuleCollider _capsule;
    private Rigidbody _rigidbody;
    private Vector2 _input;
    private Vector3 _desiredMove;
    

    [Serializable]
    public class AdvancedSettings
    {
        public float gravityMultiplier = 1.0f;
        public PhysicMaterial zeroFrictionMaterial;
        public PhysicMaterial highFrictionMaterial;
    }
    
    private void Awake()
    {
        _transform = transform;
        _capsule = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetPlayerInputAndNormalize();

        _desiredMove = _transform.forward * (_input.y * walkSpeed) + _transform.right * (_input.x * strafeSpeed);

        _rigidbody.velocity = _desiredMove + Vector3.up * _rigidbody.velocity.y;

        _capsule.material = _desiredMove.magnitude > 0 ? advanced.zeroFrictionMaterial : advanced.highFrictionMaterial;
        
        _rigidbody.AddForce(Physics.gravity * (advanced.gravityMultiplier - 1));

    }

    private void GetPlayerInputAndNormalize()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        _input = new Vector2(_horizontalInput, _verticalInput);

        if (_input.sqrMagnitude > 1)
        {
            _input.Normalize();
        }
    }
}
