using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Portal : MonoBehaviour
{
    public Portal Exit;
    private IEnumerator _coroutine;
    public GameObject MagicRing;
    private AudioSource _successPort;
    private void Awake()
    {
        _successPort = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isPortable && Exit.isPortable)
            Transport(other.gameObject);
    }
    public void Transport(GameObject obj)
    {
        _coroutine = Port(obj);
        StartCoroutine(Port(obj));
    }
    public IEnumerator Port(GameObject obj)
    {
        MagicRing.SetActive(true);
        Exit.MagicRing.SetActive(true);
        yield return new WaitForSeconds(1f);
        obj.transform.position = Exit.transform.position;
        _successPort.Play();
        Exit._successPort.Play();

        yield return new WaitForSeconds(2f);
        MagicRing.SetActive(false);
        Exit.MagicRing.SetActive(false);
        _coroutine = null;
    }
    public bool isPortable
    {
        get { return Exit._coroutine == null; }
    }
}