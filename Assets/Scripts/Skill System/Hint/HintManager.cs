using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HintManager : MonoBehaviour
{
    public RectTransform HintAnchor;
    public GameObject ScreenWord;
    public Rect TextRect;
    public static HintManager Instance;

    public Transform HintCircle;
    [HideInInspector]
    public LayerMask IgnoreOutlineRay;

    public GameObject TargetObject;
    private LayerMask _targetObjectLayer;
    public Transform CameraCenter;

    public bool HintCircleActive = false;
    public bool HintRayActive = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        HintCircle.gameObject.SetActive(false);
        IgnoreOutlineRay.value = (LayerMask.GetMask("Outline") | LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Interactable"));
    }
    private void Update()
    {
        RaycastHit hit;
        if (HintRayActive && Physics.Raycast(CameraCenter.transform.position/*, 0.1f,*/, CameraCenter.transform.forward, out hit, 10, IgnoreOutlineRay, QueryTriggerInteraction.UseGlobal))
        {
            if (hit.collider.gameObject.GetComponent<Interactable>() != null)
            {
                if (TargetObject != hit.collider.transform)
                {
                    ClearOutline(TargetObject);
                    TargetObject = hit.collider.gameObject;
                    SetOutline(TargetObject);
                }
                //HintCircle.position = hit.point;
            }
            else if(HintRayActive)
                ClearTarget();
        }
        else
            ClearTarget();

        if (HintCircleActive)
            ShowCircle();
        else
            HideCircle();
        
    }
    public void SetOutline(GameObject target)
    {
        if (target == null)
            return;
        _targetObjectLayer = target.layer;

        target.layer = LayerMask.NameToLayer("Outline");
    }
    public void ClearOutline(GameObject target)
    {
        if (target == null)
            return;
        target.layer = _targetObjectLayer;
    }
    public void ClearTarget()
    {
        ClearOutline(TargetObject);
        TargetObject = null;
    }
    public void ShowCircle()
    {
        HintCircle.gameObject.SetActive(true);
        float distance = 2.5f;
        RaycastHit hit;
        if (Physics.Raycast(CameraCenter.transform.position, CameraCenter.transform.parent.forward, out hit, distance, IgnoreOutlineRay, QueryTriggerInteraction.UseGlobal))
            distance = hit.distance;
        RaycastHit floor;
        if (Physics.Raycast(CameraCenter.transform.position + CameraCenter.transform.parent.forward * distance, Vector3.down, out floor, 100, LayerMask.GetMask("Floor"), QueryTriggerInteraction.UseGlobal))
            HintCircle.transform.position = floor.point;

    }
    public void HideCircle()
    {
        HintCircle.gameObject.SetActive(false);
    }
    /// <summary>
    /// Hint word
    /// </summary>
    /// <param name="hint"></param>
    /// <param name="hintColor"></param>
    public void ShowHint(string hint, Color hintColor)
    {
        GameObject newWord = Instantiate(ScreenWord, HintAnchor.transform.position, Quaternion.identity, HintAnchor.transform);
        newWord.GetComponent<TextMeshProUGUI>().text = hint;
        newWord.GetComponent<TextMeshProUGUI>().color = hintColor;
        newWord.AddComponent<FadeText>();
    }

    // Visualize SphereCast
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(CameraCenter.transform.position, 0.1f);
        Gizmos.DrawRay(CameraCenter.transform.position, CameraCenter.transform.forward * 10);
    }
}
