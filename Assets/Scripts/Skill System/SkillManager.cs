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

    public string Hint;
    public Color HintColor = Color.white;
    public float Delay = 0.5f;
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

    public AudioSource CastSound;
    public AudioSource CastMovement;

    private void Awake()
    {
        _capeMaterial = Cape.sharedMaterial;
        _stickMaterial = Stick.material;
        InputAction.actions["Interact"].performed += ctx => ActivateSkill();
        InputAction.actions["Skill Map"].performed += ctx => { CastSound.volume = 0.5f;  CastSound.time = 0; CastSound.Play(); };
    }
    private void Update()
    {
        if (InputAction.actions["Skill Map"].ReadValue<float>() == 1f)
            Pending();
        else if(InputAction.actions["Skill Map"].WasReleasedThisFrame())
            ClearSkillParameter();
    }
    private string[] _cursableState = new string[] { "Idle", "Turn Left", "Turn Right", "VictoryStart", "VictoryMaintain" };
    public void Pending()
    {

        if (_fadeCoroutine != null)
            return;
        bool isCursable = false;
        foreach(string state in _cursableState)
        {
            if(PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(state))
            {
                isCursable = true;
                break;
            }
        }
        if (!isCursable)
        {
            ClearSkillParameter();
            return;
        }
        _currentTime += Time.deltaTime;
        MagicRing.SetActive(true);
        PlayerAnimator.SetBool("Casting", true);
        if (_currentTime > Threshold)
            StartCoroutine(ChangeSkill());
    }
    public IEnumerator ChangeSkill()
    {
        MagicFog.SetActive(true);
        _currentTime = 0f;
        yield return new WaitForSeconds(0.5f);
        _currentSkillIndex = (_currentSkillIndex + 1) % Skills.Count;
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[_currentSkillIndex].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[_currentSkillIndex].Color);
        yield return new WaitForSeconds(2f);
        MagicFog.SetActive(false);
    }
    private string[] _useSkillState = new string[] { "Idle", "Turn Left", "Turn Right"};
    public void ActivateSkill()
    {
        bool isCursable = false;
        foreach (string state in _useSkillState)
        {
            if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(state))
            {
                isCursable = true;
                break;
            }
        }
        if (!isCursable)
            return;
        PlayerAnimator.SetTrigger("Use Skill");
        CastMovement.Play();
        PlayerAnimator.SetInteger("Skill Attack", _currentSkillIndex);
        IEnumerator coroutine = UseSkill(_currentSkillIndex);
        StartCoroutine(coroutine);
    }
    public IEnumerator UseSkill(int index)
    {
        yield return new WaitForSeconds(Skills[index].Delay);
        HintManager.Instance.ShowHint(Skills[_currentSkillIndex].Hint, Skills[_currentSkillIndex].HintColor);
        Skills[index].Skill.Activate();
    }
    public void ClearSkillParameter()
    {
        _currentTime = 0f;
        _fadeCoroutine = FadeOutEffect();
        StartCoroutine(_fadeCoroutine);
        PlayerAnimator.SetBool("Casting", false);
    }
    public IEnumerator FadeOutEffect()
    {
        Material ringMaterial = MagicRing.GetComponent<ParticleSystemRenderer>().material;
        Color color = ringMaterial.GetColor("_TintColor");
        for (float f= 0f; f<=0.5f; f += Time.deltaTime)
        {
            CastSound.volume = Mathf.Lerp(0.5f, 0f, f / 0.5f);
            ringMaterial.SetColor("_TintColor", new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0, f/0.5f)));
            yield return null;
        }
        CastSound.Pause();
        yield return new WaitForSeconds(1.0f);
        MagicRing.SetActive(false);
        ringMaterial.SetColor("_TintColor", color);

        MagicFog.SetActive(false);
        _fadeCoroutine = null;
    }
    private void OnDisable()
    {
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[0].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[0].Color);
        StopAllCoroutines();
    }
}

