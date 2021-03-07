using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public bool pickedMeUp;

    public AudioSource pickUpSound;
    
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
