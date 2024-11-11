using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
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
    [SerializeField]
    private float _currentTime = 0f;
    private IEnumerator _fadeCoroutine;
    private IEnumerator _changeSkillCoroutine;
    
    private Material _capeMaterial;
    private Material _stickMaterial;

    public AudioSource CastSound;
    public AudioSource CastMovement;
    public AudioSource LevelingSound;

    private void Awake()
    {
        _capeMaterial = Cape.sharedMaterial;
        _stickMaterial = Stick.material;
        InputAction.actions["Interact"].performed += ctx =>
        {
            string name = Skills[_currentSkillIndex].Skill.Name;
            if (name == "Aqua" || name == "Wind")
                HintManager.Instance.HintCircleActive = true;
            else
                HintManager.Instance.HintRayActive = true;
        };
        InputAction.actions["Interact"].canceled += ctx => {
            ActivateSkill();
            HintManager.Instance.HintCircleActive = false;
            HintManager.Instance.HintRayActive = false;

        };
    }
    private void Update()
    {
        if (InputAction.actions["Skill Map"].ReadValue<float>() == 1f)
            Pending();
        else if(InputAction.actions["Skill Map"].WasReleasedThisFrame())
            ClearSkillParameter();
    }
    private string[] _cursableState = new string[] { "Idle", "Turn Left", "Turn Right", "VictoryStart", "VictoryMaintain" };
    
    /// <summary>
    /// Change skill
    /// </summary>
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
        if(_changeSkillCoroutine == null)
            _currentTime += Time.deltaTime;
        
        MagicRing.SetActive(true);
        if (!PlayerAnimator.GetBool("Casting"))
        {
            CastSound.volume = 0.5f; 
            CastSound.time = 0;
            CastSound.Play();
        }
        PlayerAnimator.SetBool("Casting", true);
        // Sound Problem
        
        if (_currentTime > Threshold && _changeSkillCoroutine == null)
        {
            _changeSkillCoroutine = ChangeSkill();
            StartCoroutine(_changeSkillCoroutine);
        }
    }
    public IEnumerator ChangeSkill()
    {
        MagicFog.SetActive(true);
        _currentTime = 0f;
        LevelingSound.time = 0;
        LevelingSound.volume = 0.5f; 
        LevelingSound.Play();
        yield return new WaitForSeconds(0.5f);
        _currentSkillIndex = (_currentSkillIndex + 1) % Skills.Count;
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[_currentSkillIndex].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[_currentSkillIndex].Color);
        yield return new WaitForSeconds(2f);
        MagicFog.SetActive(false);
        _changeSkillCoroutine = null;
    }
    public IEnumerator FadeOutEffect()
    {
        List<Material> ringMaterials = new List<Material>();
        ringMaterials.Add(MagicRing.GetComponent<ParticleSystemRenderer>().material);
        for (int i = 0; i < MagicRing.transform.childCount; i++)
            ringMaterials.Add(MagicRing.transform.GetChild(i).GetComponent<ParticleSystemRenderer>().material);

        List<Color> colors = ringMaterials.Select(x => x.GetColor("_TintColor")).ToList();

        float duration = 0.5f;
        for (float f = 0f; f <= duration; f += Time.deltaTime)
        {
            CastSound.volume = Mathf.Lerp(duration, 0f, f / duration);
            LevelingSound.volume = Mathf.Lerp(duration, 0f, f / duration);
            for (int i = 0; i < colors.Count; i++)
                ringMaterials[i].SetColor("_TintColor", new Color(colors[i].r, colors[i].g, colors[i].b, Mathf.Lerp(colors[i].a, 0, f / duration)));
            yield return null;
        }
        CastSound.Pause();
        yield return null;
        MagicRing.SetActive(false);
        for (int i = 0; i < colors.Count; i++)
            ringMaterials[i].SetColor("_TintColor", colors[i]);

        MagicFog.SetActive(false);
        _fadeCoroutine = null;
        _currentTime = 0f;
    }
    public void ClearSkillParameter()
    {
        _currentTime = 0f;
        _fadeCoroutine = FadeOutEffect();
        StartCoroutine(_fadeCoroutine);
        PlayerAnimator.SetBool("Casting", false);
    }

    /// <summary>
    /// Use skill
    /// </summary>
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
        StartCoroutine(UseSkill(_currentSkillIndex));
    }
    public IEnumerator UseSkill(int index)
    {
        Skills[index].Skill.SetTarget(HintManager.Instance.TargetObject);
        yield return new WaitForSeconds(Skills[index].Delay);
        HintManager.Instance.ShowHint(Skills[_currentSkillIndex].Hint, Skills[_currentSkillIndex].HintColor);
        Skills[index].Skill.Activate();
    }
    
    private void OnDisable()
    {
        _capeMaterial.SetColor("Color_c18aea2e3ad54319abb53f299507b005", Skills[0].Color);
        _stickMaterial.SetColor("_BaseColor", Skills[0].Color);
        StopAllCoroutines();
    }
}

