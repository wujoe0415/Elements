using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillPotion : DialogueObject
{
    public string Color = "red";

    public List<string> AfterFilled = new List<string>();
    private AudioSource _audioSource;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public override void Interact()
    {
        if (CollectionBag.Instance.ContainsCollection("Potion"))
        {
            Debug.Log("Fill potion");
            Debug.Log(CollectionBag.Instance.ContainsCollection("Potion").name);
            CollectionBag.Instance.ContainsCollection("Potion").name = $"Potion_{Color}";
            Debug.Log(CollectionBag.Instance.ContainsCollection("Potion").name);
            GameObject potion = CollectionBag.Instance.ContainsCollection("Potion");
            Material[] m = potion.GetComponent<MeshRenderer>().materials;
            //Debug.Log(mixGameObject.gameObject.name.Substring(10));
            Color color = GetPotionColor(Color);

            m[m.Length - 1].color = color;
            m[m.Length - 1].EnableKeyword("_EMISSION");
            m[m.Length - 1].SetColor("_EmissionColor", color);

            potion.GetComponent<MeshRenderer>().materials = m;

            DialogueSystem.Instance.SetDiagues(AfterFilled);
            DialogueSystem.Instance.StartDialogue();
            _audioSource.Play();
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
            default:
                return new Color(0f, 0f, 0f, 1f);   // Default white
        }
    }
}
