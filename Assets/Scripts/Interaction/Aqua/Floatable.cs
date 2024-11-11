using System.Collections;
using UnityEngine;

public class Floatable : MonoBehaviour
{
    public Transform Surface;
    public float Distance = 0.05f;
    private void Start()
    {
        StartCoroutine(Floating());
    }
    private IEnumerator Floating()
    {
        float cycle = Random.Range(1f, 2f);
        float angle = 0f;
        float offsetCenter = transform.position.y - Surface.position.y;
        while (true)
        {
            angle += Time.deltaTime * (2 * Mathf.PI / cycle);
            transform.position += (Surface.position.y - transform.position.y + offsetCenter + Distance * Mathf.Sin(angle)) * Vector3.up;
            yield return null;
        }
    }
}
