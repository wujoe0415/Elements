using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SrollOPN : Interactable
{
    public GameObject HintCanvas;
    public AudioClip OpenSound;
    public AudioClip CloseSound;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void Interact()
    {
        base.Interact();
        HintCanvas.SetActive(true);
        audioSource.PlayOneShot(OpenSound);
    }
    public void CloseHint()
    {
        HintCanvas.SetActive(false);
        audioSource.PlayOneShot(CloseSound);
    }
}
