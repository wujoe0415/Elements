using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Portal : MonoBehaviour
{
    public Portal Exit;
    private IEnumerator _coroutine;
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
        if (other.tag == "Player" && _coroutine != null && !isPorted)
        {
            StopCoroutine(_coroutine);
            MagicRing.SetActive(false);
            Exit.MagicRing.SetActive(false);
            _successPort.Stop();
            Exit._successPort.Stop();
            _coroutine = null;
            isPorted = false;
        }
    }
    public void Transport(GameObject obj)
    {
        _coroutine = Port(obj);
        StartCoroutine(Port(obj));
    }
    public IEnumerator Port(GameObject obj)
    {
        isPorted = false;
        MagicRing.SetActive(true);
        Exit.MagicRing.SetActive(true);
        yield return new WaitForSeconds(1f);
        Debug.Log(obj);
        Debug.Log(obj.transform.position + " " + Exit.transform.position);
        obj.GetComponent<CharacterController>().transform.position = Exit.transform.position;
        isPorted = true;
        _successPort.Play();
        Exit._successPort.Play();

        yield return new WaitForSeconds(2f);
        MagicRing.SetActive(false);
        Exit.MagicRing.SetActive(false);
        _coroutine = null;
        isPorted = false;
    }
    public bool isPortable
    {
        get { return Exit._coroutine == null; }
    }
}
