using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : Interactable
{
    [System.Serializable]
    public class MixFormula
    {
        public string color1;
        public string color2;
        public string result;
        public MixFormula(string color1, string color2, string result)
        {
            this.color1 = color1;
            this.color2 = color2;
            this.result = result;
        }
    }

    public string Mix1;
    public string Mix2;
    public GameObject NormalResult;
    public GameObject MixResult;
    public MeshRenderer Mix1Result;
     
    public List<MixFormula> MixFormulas = new List<MixFormula>();

    public Material PotionNone;

    public override void Interact()
    {
        GameObject tmp = CollectionBag.Instance.ContainsCollection("Potion");
        if (tmp == null)
            return;
        SetMix(tmp);
    }
    public void SetMix(GameObject mix)
    {
        if(MixResult.activeInHierarchy)
            return;

        if (!string.IsNullOrEmpty(Mix1))
        {
            Mix2 = mix.name;
            Mix(Mix1, Mix2);
            SetLiquidColor(Mix1Result, "Default");
            Mix1 = "";
            Mix2 = "";
        }
        else
        {
            Mix1 = mix.name;
            SetLiquidColor(Mix1Result, mix.name.Substring(7));
        }
        Debug.Log("Use finished");
        mix.gameObject.name = "Potion_none";
        // Get the current materials array
        MeshRenderer meshRenderer = mix.GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.materials;

        // Create a new materials array and modify the last element
        materials[materials.Length - 1] = PotionNone;

        // Assign the modified array back to the renderer
        meshRenderer.materials = materials;

        CollectionBag.Instance.TakeOutCollection(mix);
    }

    public void Mix(string mix1, string mix2)
    {
        if (mix1 == null || mix2 == null)
            return;

        string mix1Name = mix1.Substring(7);
        string mix2Name = mix2.Substring(7);
        Debug.Log(mix1Name + " " + mix2Name);

        // Try to find a matching formula
        string result = FindFormulaResult(mix1Name, mix2Name);
        if (string.IsNullOrEmpty(result))
        {
            Debug.Log($"No formula found for mixing {mix1Name} and {mix2Name}");
            return;
        }

        // Handle the result
        HandleMixResult(result);
        NormalResult.SetActive(false);
    }

    private string FindFormulaResult(string ingredient1, string ingredient2)
    {
        foreach (MixFormula formula in MixFormulas)
        {
            // Check both possible orderings of ingredients
            if ((formula.color1 == ingredient1 && formula.color2 == ingredient2) ||
                (formula.color1 == ingredient2 && formula.color2 == ingredient1))
            {
                return formula.result;
            }
        }
        return "";
    }

    private void HandleMixResult(string result)
    {
        // Special handling for explosion
        if (result.ToLower() == "explosion")
        {
            // Trigger explosion effect
            HandleExplosion();
            return;
        }
        // Instantiate the result
        MixResult.SetActive(true);
        MixResult.name = "MixPotion_" + result;
        SetLiquidColor(MixResult.transform.GetChild(0).GetComponent<MeshRenderer>(), result);
        
    }

    private void HandleExplosion()
    {
        // You can implement explosion effects here
        Debug.Log("BOOM! Mixture exploded!");

        // Example: You might want to trigger a particle system
        ParticleSystem explosionEffect = GetComponentInChildren<ParticleSystem>();
        if (explosionEffect != null)
        {
            explosionEffect.Play();
        }
    }

    public void SetLiquidColor(MeshRenderer liquid, string potionName)
    {
        Material liquidMaterial = liquid.material;
        Color color = GetPotionColor(potionName);
        liquidMaterial.color = color;
        liquidMaterial.EnableKeyword("_EMISSION");
        liquidMaterial.SetColor("_EmissionColor", color);
        liquid.material = liquidMaterial;
    }
    private Color GetPotionColor(string potionName)
    {
        switch (potionName.ToLower())
        {
            case "red":
                return new Color(1f, 0f, 0f, 0.8f);  // Red with some transparency
            case "blue":
                return new Color(0f, 0.4f, 1f, 0.8f); // Bright blue
            case "green":
                return new Color(0f, 1f, 0f, 0.8f);   // Green
            case "yellow":
                return new Color(1f, 1f, 0f, 0.8f);   // Yellow
            case "pink":
                return new Color(1f, 0.4f, 0.7f, 0.8f); // Pink
            case "purple":
                return new Color(0.5f, 0f, 0.5f, 0.8f); // Purple
            default:
                return new Color(0f, 0f, 0f, 1f);   // Default white
        }
    }

    // Helper method to initialize formulas in Start or from another script
    public void InitializeFormulas()
    {
        MixFormulas.Clear();

        // Add your formulas here
        MixFormulas.Add(new MixFormula("red", "yellow", "pink"));
        MixFormulas.Add(new MixFormula("pink", "red", "blue"));
        MixFormulas.Add(new MixFormula("blue", "yellow", "green"));
        MixFormulas.Add(new MixFormula("green", "blue", "purple"));
        MixFormulas.Add(new MixFormula("purple", "red", "explosion"));
    }

    public void SwitchToNormal()
    {
        MixResult.SetActive(false);
        NormalResult.SetActive(true);
    }
    // Optional: Initialize formulas when the component starts
    private void Start()
    {
        InitializeFormulas();
        MixResult.SetActive(false);
    }
}