using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Portal : MonoBehaviour
{
    public Portal Exit;
    public GameObject MagicRing;
    private AudioSource _successPort;

    private bool isPorted = false;
    private void Awake()
    {
        _successPort = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isPortable && Exit.isPortable)
            Transport(other.gameObject);
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !isPorted)
        {
            Debug.Log("None");
            StopAllCoroutines();
            Debug.Log("After Stop");
            MagicRing.SetActive(false);
            Exit.MagicRing.SetActive(false);
            _successPort.Pause();
            Exit._successPort.Pause();
            isPorted = false;
        }
    }
    public void Transport(GameObject obj)
    {
        StartCoroutine(Port(obj));
    }
    public IEnumerator Port(GameObject obj)
    {
        isPorted = false;
        MagicRing.SetActive(true);
        Exit.MagicRing.SetActive(true);
        yield return new WaitForSeconds(1f);
        obj.GetComponent<CharacterController>().enabled = false;
        obj.transform.position = new Vector3(Exit.transform.position.x, obj.transform.position.y, Exit.transform.position.z);
        obj.GetComponent<CharacterController>().enabled = true;
        isPorted = true;
        _successPort.Play();
        Exit._successPort.Play();

        yield return new WaitForSeconds(2f);
        MagicRing.SetActive(false);
        Exit.MagicRing.SetActive(false);
        isPorted = false;
    }
    public bool isPortable
    {
        get { return !Exit.isPorted; }
    }
}
