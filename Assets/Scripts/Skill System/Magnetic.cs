using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public Transform WaypointInit; // Current Transform
    public Transform WaypointDrift;
    public Transform WaypointEnd;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            StartSplit();
        else if(Input.GetKeyDown(KeyCode.M))
            StartMerge();
    }
    private IEnumerator _coroutine;
    public void StartMerge()
    {
        if(_coroutine != null )
            StopCoroutine(_coroutine);
        _coroutine = Merge();
        StartCoroutine(_coroutine);
    }
    public void StartSplit()
    {
        if(_coroutine != null )
            StopCoroutine(_coroutine);
        _coroutine = Split();
        StartCoroutine(_coroutine);
    }
    private IEnumerator Merge()
    {
        float toDriftDuration = Random.Range(3f, 5f);
        float driftDuration = 1.2f;
        float toEndDuration = Random.Range(2f, 4f);

        Vector3 initPos = transform.position;
        Quaternion initRot = transform.rotation;
        for (float f = 0f; f <= toDriftDuration; f+=Time.deltaTime)
        {
            transform.position = Vector3.Slerp(initPos, WaypointDrift.position, f/toDriftDuration);
            transform.rotation = Quaternion.Slerp(initRot, WaypointDrift.rotation, f / toDriftDuration);
            yield return null;
        }
        // Drift
        // TODO: more smooth
        float step = 0.002f;
        for(float f = 0f;f<= driftDuration;f+=Time.deltaTime)
        {
            transform.position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * step;
            yield return null;
        }
        initPos = transform.position;
        initRot = transform.rotation;
        for (float f = 0f; f <= toEndDuration; f+=Time.deltaTime)
        {
            
            transform.position = Vector3.Slerp(initPos, WaypointEnd.position, f / toEndDuration);
            transform.rotation = Quaternion.Slerp(initRot, WaypointEnd.rotation, f / toEndDuration);
            yield return null;
        }
    }
    private IEnumerator Split()
    {
        float toDriftDuration = Random.Range(3f, 5f);
        float toEndDuration = Random.Range(2f, 4f);

        Vector3 initPos = transform.position;
        Quaternion initRot = transform.rotation;
        for (float f = 0f; f <= toDriftDuration; f += Time.deltaTime)
        {
            transform.position = Vector3.Slerp(initPos, WaypointDrift.position, f / toDriftDuration);
            transform.rotation = Quaternion.Slerp(initRot, WaypointDrift.rotation, f / toDriftDuration);
            yield return null;
        }
        initPos = transform.position;
        initRot = transform.rotation;
        for (float f = 0f; f <= toEndDuration; f += Time.deltaTime)
        {
            transform.position = Vector3.Slerp(initPos, WaypointInit.position, f / toEndDuration);
            transform.rotation = Quaternion.Slerp(initRot, WaypointInit.rotation, f / toEndDuration);
            yield return null;
        }
    }
}
