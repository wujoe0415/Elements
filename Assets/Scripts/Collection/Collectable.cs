using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public string SingleKeyword = "";
    public override void Interact()
    {
        if(!string.IsNullOrEmpty(SingleKeyword) && CollectionBag.Instance.ContainsCollection(SingleKeyword))
            return;
        CollectionBag.Instance.AddCollection(this.gameObject);

        PutCollection collectionShelves = transform.parent.GetComponent<PutCollection>();
        if (collectionShelves == null)
            return;
        collectionShelves.Collections.Find(x => x.collection == this.gameObject).isCollected = true;
    }
}
