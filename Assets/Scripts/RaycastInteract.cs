using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RaycastInteract : MonoBehaviour
{
    public Animator crossHairImage;
        
    [SerializeField] private int rayLength = 5;
   
    private static readonly int Interacting = Animator.StringToHash("Interacting");
    private RaycastHit hit;

    private IInteractable interactable;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Raycast();
    }

    private void Raycast()
    {
        Transform rayTransform;
        Vector3 fwd = (rayTransform = transform).TransformDirection(Vector3.forward);

        bool rayCastHit = Physics.Raycast(rayTransform.position, fwd, out hit, rayLength);
        Debug.DrawRay(transform.position, fwd * rayLength, Color.red);

        if (rayCastHit)
        {
            interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                crossHairImage.SetBool(Interacting, true);
                    
                Debug.Log("Interacting with Object");
                    
                if (Input.GetMouseButtonDown(0))
                {
                    interactable.Interact();
                }
            }
        }
        else
        { 
            crossHairImage.SetBool(Interacting, false);
        }
    }
}


