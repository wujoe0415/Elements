using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleBin : DialogueObject
{
    public Transform BlackSurface;
    public List<string> AfterFilled = new List<string>();

    public override void Interact()
    {
        if (CollectionBag.Instance.ContainsCollection("Potion") != null)
        {
            Debug.Log("Successfully recycled");
            if (CollectionBag.Instance.ContainsCollection("Mix") != null) // MixPotion
                DealMixPotion();
            else
                DealNormalPotion();

            DialogueSystem.Instance.SetDiagues(AfterFilled);
            DialogueSystem.Instance.StartDialogue();
            BlackSurface.gameObject.SetActive(true);
            
            if(BlackSurface.transform.position.y < 0.27f)
                BlackSurface.transform.position += Vector3.up * 0.015f;            
        }
        else
            base.Interact();
    }
    public void DealMixPotion()
    {
        GameObject potion = CollectionBag.Instance.ContainsCollection("Mix");
        potion.name = "MixPotion_none";
        Material[] m = potion.transform.GetChild(0).GetComponent<MeshRenderer>().materials;
        m[m.Length - 1].color = new Color(0f, 0f, 0f, 0.8f);
        m[m.Length - 1].DisableKeyword("_EMISSION");
        potion.transform.GetChild(0).GetComponent<MeshRenderer>().materials = m;
    }
    public void DealNormalPotion()
    {
        GameObject potion = CollectionBag.Instance.ContainsCollection("Potion");
        potion.name = $"Potion_none";
        Material[] m = potion.GetComponent<MeshRenderer>().materials;
        //Debug.Log(mixGameObject.gameObject.name.Substring(10));

        m[m.Length - 1].color = new Color(0f, 0f, 0f, 0.8f);
        m[m.Length - 1].DisableKeyword("_EMISSION");

        potion.GetComponent<MeshRenderer>().materials = m;
    }
}
