using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    public Animator crossHairImage;
    [SerializeField] private int rayLength = 5;

    private IInteractable _interactable;
    
    private static readonly int Interacting = Animator.StringToHash("Interacting");
    
    private RaycastHit _hit;
    private Transform _rayTransform;
    private Vector3 _fwd;
    private bool _rayCastHit;
    
    private void Awake()
    {
        _rayTransform = transform;
    }

    private void Start()
    {
        SetCursorState();
    }

    private void Update()
    {
        RaycastAndInteract();
    }
    
    private static void SetCursorState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RaycastAndInteract()
    {
        _fwd = _rayTransform.TransformDirection(Vector3.forward);

        _rayCastHit = Physics.Raycast(_rayTransform.position, _fwd, out _hit, rayLength);
        
        Debug.DrawRay(transform.position, _fwd * rayLength, Color.red);

        if (_rayCastHit)
        {
            _interactable = _hit.collider.gameObject.GetComponent<IInteractable>();

            if (_interactable == null) return;
            InteractingWithHitObject();
        }
        else
        { 
            crossHairImage.SetBool(Interacting, false);
        }
    }

    private void InteractingWithHitObject()
    {
        crossHairImage.SetBool(Interacting, true);
        
        if (Input.GetMouseButtonDown(0))
        {
            _interactable.Interact();
        }
    }
}


