using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FloorType
{
    Default,
    Brick,
    Forest,
    Water
}

[System.Serializable]
public class FloorSound
{
    public FloorType Floor;
    public AudioClip Walk;
    public AudioClip Run;
    public float WalkSpeed = 5f;
    public float RunSpeed = 8f;

}
[RequireComponent(typeof(AudioSource))]
public class WalkSoundManager : MonoBehaviour
{
    public Locomotion PlayerLocomotion;
    private AudioSource _audioSource;
    public List<FloorSound> SoundManager = new List<FloorSound>();
    private FloorSound _currentfloor;
    public static WalkSoundManager Instance
    {
        get
        {
            return _instance;
        }
        set { _instance = value; }
    }
    private static WalkSoundManager _instance = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Pause();
        ChangeFloor(FloorType.Default);
    }
    public FloorType GetState()
    {
        return _currentfloor.Floor;
    }
    public void ChangeFloor(FloorType floor)
    {
        foreach(FloorSound fs in SoundManager)
        {
            if(fs.Floor == floor)
            {
                _currentfloor = fs;
                break;
            }
        }
    }
    public void Update()
    {
        if (!PlayerLocomotion.isMoving || !PlayerLocomotion.isGrounded())
        {
            _audioSource.Pause();
            //_audioSource.clip = null;
            return;
        }
        else if (!_audioSource.isPlaying && PlayerLocomotion.isGrounded())
            _audioSource.UnPause();

        if (_audioSource.clip != _currentfloor.Run && PlayerLocomotion.isRun)
        {
            _audioSource.clip = _currentfloor.Run;
            _audioSource.Play();
        }
        else if (_audioSource.clip != _currentfloor.Walk && !PlayerLocomotion.isRun)
        {
            _audioSource.clip = _currentfloor.Walk;
            _audioSource.Play();
        }
    }
}
