using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Collectable
{
    public override void Interact()
    {
        GameObject mixGameObject = CollectionBag.Instance.ContainsCollection("Mix");
        if(gameObject.name.Contains("none") && mixGameObject != null)
        {
            Material[] m = GetComponent<MeshRenderer>().materials;
            Debug.Log(mixGameObject.gameObject.name.Substring(10));
            gameObject.name = $"Potion_{mixGameObject.gameObject.name.Substring(10)}";
            Color color = GetPotionColor(mixGameObject.gameObject.name.Substring(10));
            
            m[m.Length - 1].color = color;
            m[m.Length - 1].EnableKeyword("_EMISSION");
            m[m.Length - 1].SetColor("_EmissionColor", color);

            GetComponent<MeshRenderer>().materials = m;
            CollectionBag.Instance.TakeOutCollection(mixGameObject.gameObject);
            FindObjectOfType<Mixer>()?.SwitchToNormal();
        }
        else
            base.Interact();
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
            case "black":
                return new Color(0f, 0f, 0f, 0.8f); // Black
            default:
                return new Color(0f, 0f, 0f, 1f);   // Default white
        }
    }
}
