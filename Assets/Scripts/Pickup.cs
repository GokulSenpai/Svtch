using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [Space]
    [ShowOnly] public bool pickedMeUp;

    [Space]
    [SerializeField] private AudioSource pickUpSound;
    
    public void Interact()
    {
        pickedMeUp = true;
        StartCoroutine(PlayPickUpSoundAndDisableObjectCoroutine());
    }

    private IEnumerator PlayPickUpSoundAndDisableObjectCoroutine()
    {
        pickUpSound.Play();
        
        print(gameObject.name + " picked up.");
        
        yield return new WaitForSeconds(pickUpSound.time);
        gameObject.SetActive(false);
    }

   
}
