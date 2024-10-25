using System.Collections;
using System.Collections.Generic;
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
    
    private Material _capeMaterial;
    private Material _stickMaterial;

    private void Awake()
    {
        InputAction.actions["Interact"].performed += ctx => ActivateSkill();
        _capeMaterial = Cape.sharedMaterial;
        _stickMaterial = Stick.material;
    }
    private void ActivateSkill()
    {
        PlayerAnimator.SetTrigger("Use Skill");
        Skills[_currentSkillIndex].Skill.Activate();
        PlayerAnimator.SetInteger("Skill Attack", _currentSkillIndex);
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[_currentSkillIndex].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[_currentSkillIndex].Color);
        _currentSkillIndex = (_currentSkillIndex + 1) % Skills.Count;
    }
    private void OnDisable()
    {
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[0].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[0].Color);
    }
}

