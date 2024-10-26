using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SkillPackage {
    public Skill Skill;
    public Color Color;
}
public class SkillManager : MonoBehaviour
{
    public List<SkillPackage> Skills = new List<SkillPackage>();
    private int _currentSkillIndex = 0;
    public PlayerInput InputAction;
    public SkinnedMeshRenderer Cape;
    public MeshRenderer Stick;
    public Animator PlayerAnimator;

    public GameObject MagicRing;
    public GameObject MagicFog;
    public float Threshold = 2f;
    private float _currentTime = 0f;
    private IEnumerator _fadeCoroutine;
    
    private Material _capeMaterial;
    private Material _stickMaterial;

    private void Awake()
    {
        InputAction.actions["Interact"].performed += ctx => ActivateSkill();
        _capeMaterial = Cape.sharedMaterial;
        _stickMaterial = Stick.material;
    }
    private void Update()
    {
        if (InputAction.actions["Skill Map"].ReadValue<float>() == 1f)
            Pending();
        else if(InputAction.actions["Skill Map"].WasReleasedThisFrame())
            ClearSkillParameter();
    }
    public void Pending()
    {
        _currentTime += Time.deltaTime;
        MagicRing.SetActive(true);
        if (_currentTime > Threshold)
            StartCoroutine(ChangeSkill());
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
    }
    public IEnumerator ChangeSkill()
    {
        MagicFog.SetActive(true);
        _currentTime = 0f;
        yield return new WaitForSeconds(0.2f);
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[_currentSkillIndex].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[_currentSkillIndex].Color);
        _currentSkillIndex = (_currentSkillIndex + 1) % Skills.Count;
        yield return new WaitForSeconds(1.2f);
        MagicFog.SetActive(false);
    }
    public void ActivateSkill()
    {
        PlayerAnimator.SetTrigger("Use Skill");
        Skills[_currentSkillIndex].Skill.Activate();
        PlayerAnimator.SetInteger("Skill Attack", _currentSkillIndex);
        ClearSkillParameter();
    }
    public void ClearSkillParameter()
    {
        _currentTime = 0f;
        _fadeCoroutine = FadeOutEffect();
        StartCoroutine(_fadeCoroutine);
    }
    public IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(0.5f);
        MagicRing.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        MagicFog.SetActive(false);
        _fadeCoroutine = null;
    }
    private void OnDisable()
    {
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[0].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[0].Color);
    }
}

