using System.Collections;
using UnityEngine;

public class KeyPickUp : MonoBehaviour, IInteractable
{
    [Space]
    [ShowOnly] public bool pickedMeUp;

    [Space]
    [SerializeField] private AudioSource pickUpSound;
    
    public void Interact()
    {
        PickUpKey();
    }

    private void PickUpKey()
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
