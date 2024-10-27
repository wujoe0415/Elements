using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WalkSoundManager : MonoBehaviour
{
    public Locomotion PlayerLocomotion;
    private AudioSource _audioSource;
    public AudioClip WalkOnFloor;
    public AudioClip RunOnFloor;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Pause();
    }

    public void Update()
    {
        if (PlayerLocomotion.isIdle)
        {
            _audioSource.Pause();
            _audioSource.clip = null;
            return;
        }
        if (_audioSource.clip != RunOnFloor && PlayerLocomotion.isRun)
        {
            _audioSource.clip = RunOnFloor;
            _audioSource.Play();
        }
        else if (_audioSource.clip != WalkOnFloor && !PlayerLocomotion.isRun)
        {
            _audioSource.clip = WalkOnFloor;
            _audioSource.Play();
        }
    }
}
