using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public bool pickedMeUp = false;

    public AudioSource pickUpSound;

    private IEnumerator PickUpObject()
    {
        pickUpSound.Play();
        Debug.Log(gameObject.name + " picked up.");
        yield return new WaitForSeconds(pickUpSound.time);
        gameObject.SetActive(false);
    }

    public void Interact()
    {
        pickedMeUp = true;
        StartCoroutine(PickUpObject());
    }
}
